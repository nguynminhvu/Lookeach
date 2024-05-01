using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class GenderType : Audit<DateTime, Guid>
    {
        public string Name { get; private set; }
        public ICollection<Category> Categories { get; set; }

        protected GenderType(Guid id, string name, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(GenderType));
            Name = name;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

        public static GenderType Create(string name, Guid createdBy, Guid modifiedBy)
            => new GenderType(Guid.NewGuid(), name, createdBy, DateTime.Now, modifiedBy, DateTime.Now);

        public void Update(string name, Guid modifiedBy)
        {
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

    }
}
