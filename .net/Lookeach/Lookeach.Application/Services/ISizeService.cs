using Lookeach.Application.ViewModels.Sizes;

namespace Lookeach.Application.Services
{
    public interface ISizeService
    {
        Task<SizeViewModel> GetSize(Guid id);
        Task<List<SizeViewModel>> GetSizes(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<SizeViewModel> CreateSize(string name);
        Task<SizeViewModel> UpdateSize(SizeUpdateModel sizeUpdateModel);
    }
}
