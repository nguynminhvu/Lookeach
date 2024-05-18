using Lookeach.Application.Commands.Sizes;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Sizes;
using MediatR;

namespace Lookeach.Application.Handlers.Sizes
{
    public class CreateSizeCommandHandler : IRequestHandler<CreateSizeCommand, SizeViewModel>
    {
        private readonly ISizeService _sizeService;

        public CreateSizeCommandHandler(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public async Task<SizeViewModel> Handle(CreateSizeCommand request, CancellationToken cancellationToken)
        {
            var rs = await _sizeService.CreateSize(request.name);
            return rs != null ? rs : throw new Exception("Can't create new Size");
        }
    }
}
