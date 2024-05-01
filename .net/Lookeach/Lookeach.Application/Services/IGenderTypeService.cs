using Lookeach.Core.ViewModels.GenderTypes;

namespace Lookeach.Application.Services
{
    public interface IGenderTypeService
    {
        Task<GenderTypeViewModel> CreateGenderType(string name);
        Task<GenderTypeViewModel> UpdateGenderType(GenderTypeUpdateModel genderTypeUpdateModel);
        Task<GenderTypeViewModel> GetGenderType(Guid id);
        Task<List<GenderTypeViewModel>> GetGenderTypes(string? name = null, int? pageIndex = null!, int? pageSize = null!);
    }
}
