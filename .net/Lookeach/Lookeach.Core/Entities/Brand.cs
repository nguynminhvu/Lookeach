using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Brand : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<BrandImage>? BrandImages { get; set; }
        protected Brand(Guid id, string name, DateTime createdOn, DateTime modifiedOn, Guid createdBy, Guid modifiedBy)
        {
            Id = id;
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
        }

        public static Brand Create(Guid id, string name)
          => new Brand(id, name, DateTime.Now, DateTime.Now, id, id);

        public void Update(string name, Guid modifiedBy)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(Brand));
            Name = name;
            ModifiedOn = DateTime.Now;
            ModifiedBy = modifiedBy;
        }
    }
}
