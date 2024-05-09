using Lookeach.Core.DTOs;

namespace Lookeach.Application.Services
{
    public interface IActionService
    {
        Task<List<ActionDTO>> GetActionDTOs();
    }
}
