using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Services
{
    public class VoucherSourcingService : IVoucherSourcingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IVoucherSourcingRepository _voucherSourcingRepository;
        private readonly IVoucherRepository _voucherRepository;

        public VoucherSourcingService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _voucherSourcingRepository = _uow.VoucherSourcingRepository;
            _voucherRepository = _uow.VoucherRepository;
        }

        public async Task<bool> CleanVoucherSourcing(List<Guid> voucherIds)
        {
            var vouchers = await _voucherRepository.GetMany(x => voucherIds.Contains(x.Id)).ToListAsync();
            List<Guid> voucherIdsG = new List<Guid>();
            foreach (var item in vouchers)
            {
                if (item.EndTime < DateTime.Now)
                {
                    voucherIdsG.Add(item.Id);
                }
            }
            var voucherSourcings = await _voucherSourcingRepository.GetMany(x => voucherIdsG.Contains(x.VoucherId)).ToListAsync();
            _voucherSourcingRepository.RemoveRange(voucherSourcings.AsEnumerable());
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
