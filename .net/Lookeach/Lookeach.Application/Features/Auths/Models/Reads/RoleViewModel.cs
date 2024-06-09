using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.Features.Auths.Models.Reads
{
    public class RoleViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
