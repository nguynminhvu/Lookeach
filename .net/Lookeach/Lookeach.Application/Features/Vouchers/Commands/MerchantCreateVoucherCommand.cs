using Lookeach.Application.AutoMappers;
using Lookeach.Application.Interfaces;
using Lookeach.Application.ViewModels.Vouchers;
using Lookeach.Core.Entities;
using MediatR;
using System.Text;

namespace Lookeach.Application.Features.Vouchers.Commands
{
    public record MerchantCreateVoucherCommand(VoucherCreateModel VoucherCreateModel) : IRequest<VoucherViewModel>
    {
    }
    public class MerchantCreateVoucherCommandHandler : IRequestHandler<MerchantCreateVoucherCommand, VoucherViewModel>
    {
        private readonly ICleanContext _context;
        private const string VOUCHER_CODE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public MerchantCreateVoucherCommandHandler(ICleanContext cleanContext)
        {
            _context=cleanContext;
        }
        public async Task<VoucherViewModel> Handle(MerchantCreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = Voucher.Create(GenCodeVoucher(10), request.VoucherCreateModel.PriceCondition, request.VoucherCreateModel.MaxPriceOff, request.VoucherCreateModel.StartTime, request.VoucherCreateModel.EndTime, request.VoucherCreateModel.SlotEachPerson, request.VoucherCreateModel.SlotAvailable, true, false);
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync(new CancellationToken());
            return AutoMapperConfig.Mapper.Map<VoucherViewModel>(voucher);
        }
        private string GenCodeVoucher(int length)
        {
            StringBuilder voucherCode = new StringBuilder(length);
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                voucherCode.Append(VOUCHER_CODE[random.Next(VOUCHER_CODE.Length)]);
            }
            return voucherCode.ToString();
        }
    }
}
