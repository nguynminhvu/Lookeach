using Lookeach.Application.Queries.GenderTypes;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Handlers.GenderTypes
{
    public class GetGenderTypeByIdQueryHandler : IRequestHandler<GetGenderTypeByIdQuery, GenderTypeViewModel>
    {
        private readonly IGenderTypeService _genderTypeService;

        public GetGenderTypeByIdQueryHandler(IGenderTypeService genderTypeService)
        {
            _genderTypeService = genderTypeService;
        }

        public async Task<GenderTypeViewModel> Handle(GetGenderTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _genderTypeService.GetGenderType(request.id);
        }
    }
}
