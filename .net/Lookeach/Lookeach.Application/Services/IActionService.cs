using Lookeach.Application.DTOs;

namespace Lookeach.Application.Services
{
    public interface IActionService
    {
        Task<List<ActionDTO>> GetActionDTOsCache();
        Task<ClaimDTO> GetClaims(string? token = null!);
        Task SaveActionsCache();
        Task InvalidActionCache();
    }
}
