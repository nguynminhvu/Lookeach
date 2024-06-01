using Lookeach.Application.AutoMappers;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Vouchers;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using Lookeach.Core.Repositories;
using MediatR;
using System.Text;

namespace Lookeach.Application.Features.Vouchers.Commands
{
    public record SystemCreateVoucherCommand(VoucherCreateModel VoucherCreateModel) : IRequest<VoucherViewModel>
    {
    }
    public class SystemCreateVoucherCommandHandler : IRequestHandler<SystemCreateVoucherCommand, VoucherViewModel>
    {
        private readonly ICleanContext _context;
        private readonly ITokenService _tokenService;
        private const string VOUCHER_CODE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public SystemCreateVoucherCommandHandler(ITokenService tokenService, ICleanContext cleanContext)
        {
            _context = cleanContext;
            _tokenService = tokenService;
        }

        public async Task<VoucherViewModel> Handle(SystemCreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = Voucher.Create(GenCodeVoucher(10), request.VoucherCreateModel.PriceCondition, request.VoucherCreateModel.MaxPriceOff, request.VoucherCreateModel.StartTime, request.VoucherCreateModel.EndTime, request.VoucherCreateModel.SlotEachPerson, request.VoucherCreateModel.SlotAvailable, true, true);
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
