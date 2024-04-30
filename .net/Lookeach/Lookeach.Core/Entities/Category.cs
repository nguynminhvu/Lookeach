using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Category : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<CategoryImage>? CategoryImages { get; set; }
        public ICollection<GenderType>? GenderTypes { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductSourcing> ProductSourcings { get; set; }

        protected Category(Guid id, string name, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            Name = name;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public static Category Create(Guid id, string name, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
         => new Category(id, name, createdOn, createdBy, modifiedOn, modifiedBy);

        public void Update(string name, Guid modifiedBy)
        {
            Name = name;
            ModifiedOn = DateTime.Now;
            ModifiedBy = modifiedBy;
        }
    }
}
