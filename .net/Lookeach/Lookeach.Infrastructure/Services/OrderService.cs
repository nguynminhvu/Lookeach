using Lookeach.Application.Services;
using Lookeach.Core.Entities;
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
        public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpContext = httpContextAccessor;
            _voucherRepository = _uow.VoucherRepository;
            _productRepository = _uow.ProductRepository;
            _orderRepository = _uow.OrderRepository;
            _userRepository = _uow.UserRepository;
        }
        public record ProductStrucTemp(List<OrderItem> Products, decimal Amount);
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
                orderItems.Add(OrderItem.Create(orderId, item.ProductId, item.Price, item.Quantity, item.ProductVersion, claim.Id, product, product.CreatedBy));
            }

            int c = 1;
            decimal litleAmount = 0;
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
            var order = Order.Create(claim.Id, orderCreateModel.Amount, orderItems, orderCreateModel.PaymentMethodId, orderCreateModel.Address, orderCreateModel.Phone, claim.Id, orderCreateModel.Email, vouchers);
            await _orderRepository.AddAsync(order);
            await _uow.SaveChangesAsync();
            return await GetOrderById(order.Id);
        }
       
        public Task<OrderViewModel> GetOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrdersByUserId(Guid id, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderViewModel> UpdateOrder(OrderUpdateModel orderUpdateModel)
        {
            throw new NotImplementedException();
        }
    }
}
