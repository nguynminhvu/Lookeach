using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Vouchers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class VoucherService : TokenService, IVoucherService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IVoucherRepository _voucherRepository;
        private const string VOUCHER_CODE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public VoucherService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _voucherRepository = _uow.VoucherRepository;
        }

        public async Task<VoucherViewModel> CreateVoucher(VoucherCreateModel voucherCreateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");
            bool isSystem = claim.Role == RoleName.Admin.ToString();
            var voucher = Voucher.Create(GenCodeVoucher(10), voucherCreateModel.PriceCondition, voucherCreateModel.MaxPriceOff, voucherCreateModel.StartTime, voucherCreateModel.EndTime, voucherCreateModel.SlotEachPerson, voucherCreateModel.SlotAvailable, true, isSystem, claim.Id);
            await _voucherRepository.AddAsync(voucher);
            await _uow.SaveChangesAsync();
            return await GetVoucherById(voucher.Id);
        }

        public async Task<bool> DeleteVoucher(Guid id)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");
            var voucher = await _voucherRepository.GetVoucherById(id);
            if (voucher == null!) throw new ArgumentNullException("Voucher invalid man");
            voucher.DeleteVoucher(false);
            _voucherRepository.Update(voucher);
            var rs = await _uow.SaveChangesAsync();
            return rs > 0;
        }

        public async Task<List<VoucherViewModel>> GetVoucherByUserId(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<List<VoucherViewModel>>(await _voucherRepository.GetVoucherByUserId(id));
        }

        public async Task<VoucherViewModel> UpdateVoucher(VoucherUpdateModel voucherUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");
            var voucher = await _voucherRepository.GetVoucherById(voucherUpdateModel.Id);
            if (voucher == null!) throw new ArgumentNullException("Voucher invalid man");
            voucher.UpdateVoucher(voucherUpdateModel.PriceCondition, voucherUpdateModel.MaxPriceOff, voucherUpdateModel.StartTime, voucherUpdateModel.EndTime, voucherUpdateModel.SlotEachPerson, voucherUpdateModel.SlotAvailable, claim.Id);
            _voucherRepository.Update(voucher);
            await _uow.SaveChangesAsync();
            return await GetVoucherById(voucher.Id);
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

        public async Task<VoucherViewModel> GetVoucherById(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<VoucherViewModel>(await _voucherRepository.GetVoucherById(id));
        }
        public async Task<List<VoucherViewModel>> GetVouchersSystem()
        {
            return AutoMapperConfig.Mapper.Map<List<VoucherViewModel>>(await _voucherRepository.GetMany(x => x.IsSystem && x.EndTime > DateTime.Now).ToListAsync());
        }
    }
}
