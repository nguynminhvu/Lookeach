using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Color : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductSourcing> ProductSourcings { get; set; }

        protected Color(Guid id, string name, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name not null or empty");
            Name = name;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            CreatedOn = createdOn;
        }

        public static Color Create(string name, Guid createdBy)
             => new Color(Guid.NewGuid(), name, createdBy, DateTime.Now, createdBy, DateTime.Now);


        public void Update(string name, Guid modifiedBy)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name not null or empty");
            Name = name;
            ModifiedOn = DateTime.Now;
            ModifiedBy = modifiedBy;
        }
    }
}
