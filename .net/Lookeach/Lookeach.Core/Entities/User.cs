using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class User : Audit<DateTime, Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Thumbnail { get; set; }
        public bool EmailConfirm { get; set; }
        public string Password { get; set; }
        public bool IsActived { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<ChatUser>? ChatUsers { get; set; }
        private User()
        {

        }
        protected User(Guid id, string username, string name, DateTime dob, string? address, string gender, string email, string? phone, string password, Guid roleId, Guid createBy, Guid modifiedBy, DateTime createOn, DateTime modifiedOn)
        {
            Username = username;
            Id = id;
            Name = name;
            Dob = dob;
            if (address != null)
            {
                Address = address;
            }
            Gender = gender;
            if (phone != null)
            {
                Phone = phone;
            }
            Email = email;
            Password = password;
            RoleId = roleId;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedBy = createBy;
            CreatedOn = createOn;
        }

        public static User CreateUser(Guid id, string username, string name, DateTime dob, string? address, string gender, string email, string? phone, string password, Guid roleId, Guid createdBy, Guid modifiedBy)
            => new User(id, username, name, dob, address, gender, email, phone, password, roleId, createdBy, modifiedBy, DateTime.Now, DateTime.Now);
    }
}
