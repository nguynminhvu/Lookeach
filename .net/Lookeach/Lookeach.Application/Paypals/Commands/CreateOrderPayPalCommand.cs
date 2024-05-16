using Lookeach.Application.DTOs;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Application.Paypals.Models;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Lookeach.Application.Paypals.Commands
{
    public class CreateOrderPayPalCommand : IRequest<string>
    {
        public CreateOrderPayPalCommand(Guid orderId)
        {
            OrderId = orderId;
        }
        public Guid OrderId { get; set; }
    }

    public class CreateOrderPayPalCommandHandler : IRequestHandler<CreateOrderPayPalCommand, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IPayPalService _payPalService;
        private readonly ICleanContext _context;
        private ApplicationContext _applicationContext = new ApplicationContext();
        private readonly IIdentityUser<Guid> _identityUser;
        private ConcurrencyDTO _concurrencyDTO = new ConcurrencyDTO();

        public CreateOrderPayPalCommandHandler(ICleanContext cleanContext, IPayPalService payPalService, IConfiguration configuration, IIdentityUser<Guid> identityUser)
        {
            _identityUser = identityUser;
            _configuration = configuration;
            _payPalService = payPalService;
            _context = cleanContext;
            _configuration.GetSection(nameof(ApplicationContext)).Bind(_applicationContext);
            _configuration.GetSection(nameof(ConcurrencyDTO)).Bind(_concurrencyDTO);
        }

        // Separate concern, do not call httpClient in ưew
        public async Task<string> Handle(CreateOrderPayPalCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.OrderId);
            var paymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(x => x.Name == PaymentMethodEnum.PayPal.ToString());
            if (paymentMethod == null!) throw new ArgumentNullException("Not found PayPal payment method");

            if (order == null) throw new ArgumentNullException("Not found order");

            PayPalOrderModel paypalOrder = new PayPalOrderModel();
            paypalOrder.intent = "CAPTURE";

            ItemTotal itemTotal = new ItemTotal
            {
                currency_code = "USD",
                value = order.DollarAmount
            };

            Amount amount = new Amount
            {
                breakdown = new Breakdown
                {
                    item_total = itemTotal
                },
                value = order.DollarAmount,
                currency_code = "USD"
            };

            List<PurchaseUnit> purchase_units = new List<PurchaseUnit>();
            //   List<Item> items = new List<Item>();
            List<PayPalItem> payPalItems = new List<PayPalItem>();
            Guid paypalOrderId = Guid.NewGuid();

            foreach (var item in order.OrderItems)
            {
                //UnitAmount unitAmount = new UnitAmount
                //{
                //    currency_code = "USD",
                //    value = item.DollarPrice
                //};

                //items.Add(new Item
                //{
                //    name = item.Product.Name,
                //    description = "Lookeach product order item",
                //    quantity = item.Quantity,
                //    unit_amount = unitAmount
                //});

                payPalItems.Add(PayPalItem.Create(Guid.NewGuid(), item.Product.Name, "Lookeach product order item", item.Quantity, "USD", item.DollarPrice, paypalOrderId,_identityUser.UserId));
            }

            purchase_units.Add(new PurchaseUnit
            {
                amount = amount
                //items = items
            });

            paypalOrder.purchase_units = purchase_units;
            paypalOrder.application_context = _applicationContext;
            var responseBodyString = await _payPalService.CreateOrderPayPal(paypalOrder);

            JObject jObj = JObject.Parse(responseBodyString);

            int a = 0;
            if (await _context.PayPalOrders.Where(x => x.Token == jObj["id"] + "").AsQueryable().FirstOrDefaultAsync() != null)
            {
                foreach (var item in jObj["links"]!)
                {
                    // a==1 url redirect
                    if (a == 1)
                    {
                        return item["href"] + "";
                    }
                    a++;
                }
            }
            string merchantId = "", payeeEmail = "";
            int i = 0;
            foreach (var item in jObj["purchase_units"]!)
            {
                i++;
                merchantId = item["payee"]!["merchant_id"] + "";
                if (i == 1) break;
            }

            PayPalOrder paypal = PayPalOrder.Create(paypalOrderId, jObj["id"] + "", paypalOrder.intent, order.Status, payeeEmail, merchantId, _concurrencyDTO.ConversionRate, order.Id, _identityUser.UserId);

            PayPalAmount paypalAmount = new PayPalAmount
            {
                CurrencyCode = "USD",
                Id = Guid.NewGuid(),
                ItemTotalCurrencyCode = "USD",
                ItemTotalValue = order.DollarAmount,
                PaypalOrderId = paypal.Id,
                Value = order.DollarAmount,
            };

            TransactionMaster transactionMaster = TransactionMaster.Create(paypal.Id, order.Id, paymentMethod.Id, false, _identityUser.UserId);

            await _context.PayPalOrders.AddAsync(paypal);
            await _context.PayPalItems.AddRangeAsync(payPalItems);
            await _context.PayPalAmounts.AddAsync(paypalAmount);
            await _context.TransactionMasters.AddAsync(transactionMaster);
            await _context.SaveChangesAsync(new CancellationToken());

            a = 0;
            foreach (var item in jObj["links"]!)
            {
                if (a == 1)
                {
                    return item["href"] + "";
                }
                a++;
            }
            return null!;
        }
    }
}
