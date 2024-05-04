using MediatR;

namespace Lookeach.Application.Commands.Products
{
    public record DeleteProductCommand(Guid id) : IRequest<bool>
    {
    }
}
