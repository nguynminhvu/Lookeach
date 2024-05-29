using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Users
{
    public class UserViewModel : Audit<DateTime, Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Thumbnail { get; set; }
        public bool IsActived { get; set; }
        public RoleViewModel Role { get; set; }
    }
}
