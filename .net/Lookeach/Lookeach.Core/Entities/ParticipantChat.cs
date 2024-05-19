using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ParticipantChat : Audit<DateTime, Guid>
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ChatId { get; set; }
    }
}
