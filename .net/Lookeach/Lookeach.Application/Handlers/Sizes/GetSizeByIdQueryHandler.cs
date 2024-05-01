using Lookeach.Application.Queries.Sizes;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Sizes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
