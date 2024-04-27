using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class AccessToken : Audit<DateTime, Guid>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }

        protected AccessToken(Guid userId, string token,string refreshToken, DateTime createdOn, DateTime modifiedOn, Guid modifiedBy, Guid createdBy)
        {
            UserId = userId;
            Token = token;
            RefreshToken = refreshToken;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;

        }
        public static AccessToken Create(Guid userId, string token, string refreshToken) => new AccessToken(userId, token, refreshToken, DateTime.Now, DateTime.Now, userId, userId);
    }
}
