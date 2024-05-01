using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Size : Audit<DateTime, Guid>
    {
        public string Name { get; private set; }
        public ICollection<Product> Products { get; private set; }

        protected Size(Guid id, string name, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            Name = name;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

        public static Size Create(string name, Guid createdBy)
          => new Size(Guid.NewGuid(), name, createdBy, DateTime.Now, createdBy, DateTime.Now);


        public void UpdateSize(string name, Guid modifiedBy)
        {
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }
    }
}
