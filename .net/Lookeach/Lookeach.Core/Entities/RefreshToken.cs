using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class RefreshToken : Audit<DateTime, Guid>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public User User { get; set; }

        protected RefreshToken(Guid userId, string token, DateTime createdOn, DateTime modifiedOn, Guid modifiedBy, Guid createdBy)
        {
            UserId = userId;
            Token = token;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;

        }
        public static RefreshToken Create(Guid userId, string token) => new RefreshToken(userId, token, DateTime.Now, DateTime.Now, userId, userId);
    }
}
