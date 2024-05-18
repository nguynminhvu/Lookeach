using Lookeach.Application.Queries.Sizes;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Handlers.Sizes
{
    public class GetSizesQueryHandler : IRequestHandler<GetSizesQuery, List<SizeViewModel>>
    {
        private readonly ISizeService _sizeService;

        public GetSizesQueryHandler(ISizeService sizeService)
        {
            _sizeService= sizeService;
        }
        public async Task<List<SizeViewModel>> Handle(GetSizesQuery request, CancellationToken cancellationToken)
        {
            return await _sizeService.GetSizes(request.name, request.pageIndex, request.pageSize);
        }
    }
}
