using Lookeach.Application.Queries.Sizes;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Handlers.Sizes
{
    public class GetSizeByIdQueryHandler : IRequestHandler<GetSizeByIdQuery, SizeViewModel>
    {
        private readonly ISizeService _sizeService;

        public GetSizeByIdQueryHandler(ISizeService sizeService)
        {
            _sizeService= sizeService;
        }
        public async Task<SizeViewModel> Handle(GetSizeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _sizeService.GetSize(request.id);
        }
    }
}
