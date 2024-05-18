using Lookeach.Application.Queries.Colors;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Colors;
using MediatR;

namespace Lookeach.Application.Handlers.Colors
{
    public class GetColorsQueryHandler : IRequestHandler<GetColorsQuery, List<ColorViewModel>>
    {
        private readonly IColorService _colorService;

        public GetColorsQueryHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<List<ColorViewModel>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
        {
            return await _colorService.GetColors(request.name, request.pageIndex, request.pageSize);
        }
    }
}
