using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{

    public class OrderService : TokenService, IOrderService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherSourcingRepository _voucherSourcingRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private ConcurrencyDTO _concurrencyDTO = new ConcurrencyDTO();

        public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpContext = httpContextAccessor;
            _voucherRepository = _uow.VoucherRepository;
            _productRepository = _uow.ProductRepository;
            _orderRepository = _uow.OrderRepository;
            _userRepository = _uow.UserRepository;
            _voucherSourcingRepository = _uow.VoucherSourcingRepository;
            _paymentMethodRepository = _uow.PaymentMethodRepository;
            _configuration.GetSection(nameof(ConcurrencyDTO)).Bind(_concurrencyDTO);
        }
        public record ProductStrucTemp(List<OrderItem> Products, decimal Amount);

        private decimal ConvertToDollar(decimal amount)
        {
            amount += (amount * (Math.Round((decimal)_concurrencyDTO.ExchangeFee / 100, 2)));
            decimal result = Math.Round(amount / _concurrencyDTO.ConversionRate, 2);
            return result;
        }

        public async Task<OrderViewModel> CreateOrder(OrderCreateModel orderCreateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException();

            Guid orderId = Guid.NewGuid();
            List<Voucher> vouchers = null!;
            List<OrderItem> orderItems = new List<OrderItem>();
            List<OrderItem> productShop = new List<OrderItem>();
            Dictionary<Guid, int> productIds = new Dictionary<Guid, int>();
            Dictionary<Guid, ProductStrucTemp> productCal = new Dictionary<Guid, ProductStrucTemp>();
            HashSet<Guid> userIds = new HashSet<Guid>();
            List<User> users = new List<User>();
            List<VoucherSourcing> vouchersSourcings = new List<VoucherSourcing>();


            if (orderCreateModel.VoucherIds != null && orderCreateModel.VoucherIds.Count > 0)
            {
                vouchers = new List<Voucher>();
                vouchers = await _voucherRepository.GetMany(x => orderCreateModel.VoucherIds.Contains(x.Id)).ToListAsync();
            }

            foreach (var item in orderCreateModel.OrderItems)
            {
                productIds.Add(item.ProductId, item.Quantity);
            }

            var products = await _productRepository.GetProductByIds(productIds.Keys.ToList());
            if (products == null! || products.Count == 0) throw new Exception("Product invalid, can't create order");

            foreach (var item in orderCreateModel.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId)!;
                userIds.Add(product.CreatedBy);
                orderItems.Add(OrderItem.Create(orderId, item.ProductId, item.Price, item.DollarPrice, item.Quantity, item.ProductVersion, claim.Id, product, product.CreatedBy));
            }
            users = await _userRepository.GetUsers(userIds.ToList(), "Role");
            int c = 1;
            decimal litleAmount = 0;
            if (orderItems.Count == 1)
            {
                productShop.Add(orderItems[c - 1]);
                litleAmount = productShop[c - 1].Quantity * productShop[c - 1].Price;
                productCal.Add(products[c - 1].CreatedBy, new ProductStrucTemp(orderItems, litleAmount));
            }
            else
            {
                while (true)
                {
                    if (orderItems[c].CreatedBy == orderItems[c - 1].CreatedBy)
                    {
                        productShop.Add(orderItems[c - 1]);
                        litleAmount = productShop[c - 1].Quantity * productShop[c - 1].Price;
                        if (c == orderItems.Count - 1)
                        {
                            productShop.Add(orderItems[c]);
                            litleAmount = productShop[c].Quantity * productShop[c].Price;
                        }
                    }
                    else
                    {
                        productCal.Add(products[c - 1].CreatedBy, new ProductStrucTemp(orderItems, litleAmount));
                        productShop = new List<OrderItem>();
                        litleAmount = 0;
                    }
                    c++;
                    if (c == products.Count)
                    {
                        break;
                    }
                }
            }
            foreach (var item in products)
            {
                if (productIds.ContainsKey(item.Id))
                {
                    if (item.Quantity - productIds[item.Id] < 0)
                    {
                        throw new Exception($"Product {item.Name} not enough quantity");
                    }
                    else
                    {
                        item.UpdateInventory(item.Quantity - productIds[item.Id], claim.Id);
                    }
                }
            }
            _productRepository.UpdateRange(products.AsEnumerable());
            var paymentMethods = await _paymentMethodRepository.GetMany(x => orderCreateModel.PaymentMethodIds.Contains(x.Id)).ToListAsync();
            vouchersSourcings = await _voucherSourcingRepository.GetMany(x => x.UserId == claim.Id).ToListAsync();
            var order = Order.Create(claim.Id, orderCreateModel.Amount, ConvertToDollar(orderCreateModel.Amount), orderItems, paymentMethods, orderCreateModel.Address, orderCreateModel.Phone, claim.Id, orderCreateModel.Email, vouchers);
            var voucherSystems = await _voucherRepository.GetVouchersSystem();

            order.UpdateAmount(await ValidVoucher(vouchers, productCal, users, new decimal[] { order.Amount, order.DollarAmount }, vouchersSourcings, claim.Id, voucherSystems));

            await _orderRepository.AddAsync(order);
            await _uow.SaveChangesAsync();
            return await GetOrderById(order.Id);
        }

        private async Task<decimal[]> ValidVoucher(List<Voucher> vouchers, Dictionary<Guid, ProductStrucTemp> valuePairs, List<User> users, decimal[] totalAmount, List<VoucherSourcing> voucherSourcings, Guid userId, List<Voucher> systemVouchers)
        {
            decimal[] totalArray = totalAmount;
            decimal total = 0;
            decimal totalAmount1 = totalAmount[0];
            int voucherSystem = 0;
            if (vouchers == null!) return totalAmount;
            foreach (var item in vouchers)
            {
                if (item.StartTime > item.EndTime || item.EndTime < DateTime.Now) throw new Exception("Voucher expired time");
                if (item.CurrentSlot >= item.SlotAvailable) throw new Exception("Voucher slot not enough slot ");
                if (valuePairs.ContainsKey(item.CreatedBy))
                {
                    var user = users.FirstOrDefault(x => x.Id == item.CreatedBy);
                    if (user == null) throw new Exception("Can't identify voucher");
                    var vchSourcing = voucherSourcings.Where(x => x.VoucherId == item.Id).ToList();
                    if (vchSourcing.Count >= item.SlotEachPerson)
                    {
                        throw new Exception("User has not enough voucher slot");
                    }
                    var itemOrder = valuePairs[item.CreatedBy];
                    if ((itemOrder.Amount) * ((decimal)(item.LevelOff / 100)) > item.MaxPriceOff)
                    {
                        total += item.MaxPriceOff;
                    }
                    else
                    {
                        total += (itemOrder.Amount) * ((decimal)(item.LevelOff / 100));
                    }
                    await _voucherSourcingRepository.AddAsync(VoucherSourcing.Create(userId, item.Id, userId));
                    item.UpdateCurrentSlot(item.CurrentSlot + 1, userId);
                    _voucherRepository.Update(item);
                }
                else if (systemVouchers.Where(x => x.Id == item.Id).Count() > 0)
                {
                    var vchSourcing = voucherSourcings.Where(x => x.VoucherId == item.Id).ToList();
                    if (vchSourcing.Count >= item.SlotEachPerson)
                    {
                        throw new Exception("User has not enough voucher slot");
                    }
                    if (totalAmount1 * ((decimal)item.LevelOff / 100) > item.MaxPriceOff)
                    {
                        totalAmount1 -= item.MaxPriceOff;
                    }
                    else
                    {
                        totalAmount1 -= totalAmount1 * ((decimal)item.LevelOff / 100);
                    }
                    await _voucherSourcingRepository.AddAsync(VoucherSourcing.Create(userId, item.Id, userId));
                    item.UpdateCurrentSlot(item.CurrentSlot + 1, userId);
                    _voucherRepository.Update(item);
                    voucherSystem++;
                }
                if (voucherSystem > 1)
                {
                    throw new Exception("Voucher system accept at most 1");
                }
            }
            totalAmount1 -= total;
            var dollarAmount = totalAmount[1] - totalAmount[1] * (Math.Round((totalAmount[0] - totalAmount1) / totalAmount[0], 2));
            totalArray = new[] { totalAmount1, dollarAmount };
            return totalArray;
        }


        public async Task<OrderViewModel> GetOrderById(Guid id)
        => AutoMapperConfig.Mapper.Map<OrderViewModel>(await _orderRepository.FirstOrDefaultAsync(x => x.Id == id, "PaymentMethod,OrderItems,Vouchers"));

        public async Task<List<OrderViewModel>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!)
        => AutoMapperConfig.Mapper.Map<List<OrderViewModel>>(await _orderRepository.GetOrders(dateFrom, dateTo, pageIndex, pageSize, null!));

        public async Task<List<OrderViewModel>> GetOrdersByUserId(Guid id, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null!, int? pageSize = null!)
        => AutoMapperConfig.Mapper.Map<List<OrderViewModel>>(await _orderRepository.GetOrdersByUserId(id, dateFrom, dateTo, pageIndex, pageSize, null!));

        public async Task<OrderViewModel> UpdateOrder(OrderUpdateModel orderUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login expired time");

            var order = await _orderRepository.FirstOrDefaultAsync(x => x.Id == orderUpdateModel.Id);
            if (order == null!) throw new Exception("Order not found");
            order.UpdateStatus(orderUpdateModel.Status, claim.Id);
            _orderRepository.Update(order);
            await _uow.SaveChangesAsync();
            return await GetOrderById(order.Id);
        }
    }
}
