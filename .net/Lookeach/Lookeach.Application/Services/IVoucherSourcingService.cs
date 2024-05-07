namespace Lookeach.Application.Services
{
    public interface IVoucherSourcingService
    {
        Task<bool> CleanVoucherSourcing(List<Guid> voucherIds);
    }
}
