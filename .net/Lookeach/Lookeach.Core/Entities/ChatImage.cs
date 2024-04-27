using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ChatImage : BaseEntity<Guid>
    {
        public Guid ChatId { get; set; }
        public Guid ImageId { get; set; }

        public Chat Chat { get; set; }
        public Image Image { get; set; }
    }
}
