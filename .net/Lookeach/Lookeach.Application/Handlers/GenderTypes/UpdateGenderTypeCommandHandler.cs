using Lookeach.Application.Commands.GenderTypes;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.GenderTypes;
using MediatR;

namespace Lookeach.Application.Handlers.GenderTypes
{
    public class UpdateGenderTypeCommandHandler : IRequestHandler<UpdateGenderTypeCommand, GenderTypeViewModel>
    {
        private readonly IGenderTypeService _genderTypeService;

        public UpdateGenderTypeCommandHandler(IGenderTypeService genderTypeService)
        {
            _genderTypeService = genderTypeService;
        }
        public async Task<GenderTypeViewModel> Handle(UpdateGenderTypeCommand request, CancellationToken cancellationToken)
        {
            var rs = await _genderTypeService.UpdateGenderType(request.GenderTypeUpdateModel);
            return rs != null ? rs : throw new Exception("Can't update this gender type");
        }
    }
}
