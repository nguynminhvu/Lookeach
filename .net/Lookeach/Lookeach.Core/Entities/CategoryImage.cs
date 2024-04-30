using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class CategoryImage : BaseEntity<Guid>
    {
        public Guid CategoryId { get; set; }
        public Guid ImageId { get; set; }

        public Category Category { get; set; }
        public Image Image { get; set; }

        protected CategoryImage(Guid id, Guid categoryId, Guid imageId)
        {
            Id = id;
            CategoryId = categoryId;
            ImageId = imageId;
        }

        public static CategoryImage Create(Guid categoryId, Guid imageId)
            => new CategoryImage(Guid.NewGuid(), categoryId, imageId);
    }
}
