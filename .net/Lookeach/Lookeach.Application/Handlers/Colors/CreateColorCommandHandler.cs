using Lookeach.Application.Commands.Colors;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Handlers.Colors
{
    public class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, ColorViewModel>
    {
        private readonly IColorService _colorService;

        public CreateColorCommandHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<ColorViewModel> Handle(CreateColorCommand request, CancellationToken cancellationToken)
        {
            var rs = await _colorService.CreateColor(request.name);
            return rs != null ? rs : throw new Exception("Can't create new Color");
        }
    }
}
