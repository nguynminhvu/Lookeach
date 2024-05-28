using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class SuperAdmin:Audit<DateTime,Guid>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
