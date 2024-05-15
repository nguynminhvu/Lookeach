using Lookeach.Application.AutoMappers;
using Lookeach.Application.Exceptions;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Core.Enums;
using Lookeach.Core.ViewModels.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Lookeach.Application.Paypals.Commands
{
    public class CompleteOrderPayPalCommand : IRequest<OrderViewModel>
    {
        public string Token { get; set; }
        public CompleteOrderPayPalCommand(string token)
        {
            Token = token;
        }
    }
    public class CompleteOrderPayPalCommandHandler : IRequestHandler<CompleteOrderPayPalCommand, OrderViewModel>
    {
        private readonly ICleanContext _context;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IPayPalService _payPalService;

        public CompleteOrderPayPalCommandHandler(IIdentityUser<Guid> identityUser, IPayPalService payPalService, ICleanContext cleanContext)
        {
            _context = cleanContext;
            _identityUser = identityUser;
            _payPalService = payPalService;
        }

        public async Task<OrderViewModel> Handle(CompleteOrderPayPalCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token)) throw new PayPalException("This PayPal transaction invalid");

            var paypalOrder = await _context.PayPalOrders.FirstOrDefaultAsync(x => x.Token == request.Token);

            if (paypalOrder == null!) throw new PayPalException("Not found this PayPal transaction");
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == paypalOrder.OrderId);
            if (order == null!) throw new Exception("Not found order");

            var transactionMaster = await _context.TransactionMasters.FirstOrDefaultAsync(x => x.Id == paypalOrder.Id && !x.IsCompleted);

            if (transactionMaster == null) throw new PayPalException("Not found this PayPal transaction");

            string responseBody = await _payPalService.CompletedOrder(request.Token);

            JObject jObj = JObject.Parse(responseBody);

            if (jObj["id"] + "" != paypalOrder.Token) throw new PayPalException("This PayPal transaction invalid");

            var captureId = "";

            foreach (var item in jObj["purchase_units"]!)
            {
                foreach (var item1 in item["payments"]!["captures"]!)
                {
                    captureId = item1["id"] + "";
                    break;
                }
            }
            transactionMaster.IsCompleted = true;
            paypalOrder.UpdatePayPaylOrderCompleted(captureId, OrderStatus.Done.ToString(), _identityUser.UserId);

            _context.TransactionMasters.Update(transactionMaster);
            _context.PayPalOrders.Update(paypalOrder);

            await _context.SaveChangesAsync(new CancellationToken());

            return AutoMapperConfig.Mapper.Map<OrderViewModel>(order);
        }
    }
}
