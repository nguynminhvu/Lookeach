using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IImageRepository:IRepository<Image>
    {
        Task<List<Image>> GetImagesByListId(List<Guid> imageId); 
    }
}
