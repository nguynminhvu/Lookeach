using MediatR;

namespace Lookeach.Application.Commands.VNPays
{
    public record GetUrlVNPayCommand(Guid Id) : IRequest<string>
    {
    }
}
