using Lookeach.Application.Commands.Colors;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Handlers.Colors
{
    public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, ColorViewModel>
    {
        private readonly IColorService _colorService;

        public UpdateColorCommandHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<ColorViewModel> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            var rs = await _colorService.UpdateColor(request.ColorUpdateModel);
            return rs != null! ? rs : throw new Exception("Can't update this Color");
        }
    }
}
