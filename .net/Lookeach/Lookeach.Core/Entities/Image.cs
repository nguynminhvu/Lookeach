using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Image : Audit<DateTime, Guid>
    {
        public string Url { get; set; }

        public Image(Guid id, string url, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("Url is required");
            Url = url;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedOn = createdOn;
        }

        public static Image Create(string url, Guid createdBy, Guid modifiedBy)
            => new Image(Guid.NewGuid(), url, createdBy, DateTime.Now, modifiedBy, DateTime.Now);
    }
}
