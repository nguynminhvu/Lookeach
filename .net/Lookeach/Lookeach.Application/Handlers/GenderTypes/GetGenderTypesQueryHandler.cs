using Lookeach.Application.Queries.GenderTypes;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Handlers.GenderTypes
{
    public class GetGenderTypesQueryHandler : IRequestHandler<GetGenderTypesQuery, List<GenderTypeViewModel>>
    {
        private readonly IGenderTypeService _genderTypeService;

        public GetGenderTypesQueryHandler(IGenderTypeService genderTypeService)
        {
            _genderTypeService = genderTypeService;
        }
        public async Task<List<GenderTypeViewModel>> Handle(GetGenderTypesQuery request, CancellationToken cancellationToken)
        {
            return await _genderTypeService.GetGenderTypes(request.name, request.pageIndex, request.pageSize);
        }
    }
}
