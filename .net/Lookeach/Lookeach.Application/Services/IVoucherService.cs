using Lookeach.Core.ViewModels.Vouchers;

namespace Lookeach.Application.Services
{
    public interface IVoucherService
    {
        Task<VoucherViewModel> CreateVoucher(VoucherCreateModel voucherCreateModel);
        Task<VoucherViewModel> UpdateVoucher(VoucherUpdateModel voucherUpdateModel);
        Task<List<VoucherViewModel>> GetVoucherByUserId(Guid id);
        Task<bool> DeleteVoucher(Guid id);
        Task<VoucherViewModel> GetVoucherById(Guid id);
        Task<List<VoucherViewModel>> GetVouchersSystem();
    }
}
