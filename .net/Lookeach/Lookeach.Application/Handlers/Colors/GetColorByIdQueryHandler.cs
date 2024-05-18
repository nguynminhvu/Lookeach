using Lookeach.Application.Queries.Colors;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Handlers.Colors
{
    public class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, ColorViewModel>
    {
        private readonly IColorService _colorService;

        public GetColorByIdQueryHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<ColorViewModel> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _colorService.GetColorById(request.id);
        }
    }
}
