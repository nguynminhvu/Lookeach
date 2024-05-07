using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class VoucherRepository : Repository<Voucher>, IVoucherRepository
    {
        private readonly LookeachContext _context;
        public VoucherRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Voucher> GetVoucherById(Guid id)
        => (await _context.Vouchers.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<List<Voucher>> GetVoucherByUserId(Guid id)
        => await _context.Vouchers.Where(x => x.CreatedBy == id).ToListAsync();

        public async Task<List<Voucher>> GetVouchersSystem()
        => await _context.Vouchers.Where(x => x.IsSystem && x.EndTime >= DateTime.Now).ToListAsync();
    }
}
