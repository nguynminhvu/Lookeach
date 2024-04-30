using Lookeach.Application.Commands.GenderTypes;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Handlers.GenderTypes
{
    public class CreateGenderTypeCommandHandler : IRequestHandler<CreateGenderTypeCommand, GenderTypeViewModel>
    {
        private readonly IGenderTypeService _genderTypeService;

        public CreateGenderTypeCommandHandler(IGenderTypeService genderTypeService)
        {
            _genderTypeService=genderTypeService;
        }
        public async Task<GenderTypeViewModel> Handle(CreateGenderTypeCommand request, CancellationToken cancellationToken)
        {
            var rs = await _genderTypeService.CreateGenderType(request.name);
            return rs != null ? rs : throw new Exception("Can't create new Gender Type");
        }
    }
}
