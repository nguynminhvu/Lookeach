using Lookeach.Application.Commands.Sizes;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Handlers.Sizes
{
    public class UpdateSizeCommandHandler : IRequestHandler<UpdateSizeCommand, SizeViewModel>
    {
        private readonly ISizeService _sizeService;

        public UpdateSizeCommandHandler(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public async Task<SizeViewModel> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
        {
            var rs = await _sizeService.UpdateSize(request.SizeUpdateModel);
            return rs != null ? rs : throw new Exception("Can't update Size");
        }
    }
}
