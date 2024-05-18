using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<List<Voucher>> GetVoucherByUserId(Guid id);
        Task<Voucher> GetVoucherById(Guid id);
        Task<List<Voucher>> GetVouchersSystem();
    }
}
