using Lookeach.Application.ViewModels.Colors;

namespace Lookeach.Application.Services
{
    public interface IColorService
    {
        Task<ColorViewModel> GetColorById(Guid id);
        Task<List<ColorViewModel>> GetColors(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<ColorViewModel> CreateColor(string name);
        Task<ColorViewModel> UpdateColor(ColorUpdateModel colorUpdateModel);
    }
}
