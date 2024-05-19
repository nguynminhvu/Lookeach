using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Application.Features.Users.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Role is require")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Username at least 5 character")]
        [MinLength(5, ErrorMessage = "Username at least 5 character")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password at least 6 character")]
        [MinLength(5, ErrorMessage = "Password at least 6 character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password at least 2 character")]
        [MinLength(5, ErrorMessage = "Password at least 2 character")]
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
