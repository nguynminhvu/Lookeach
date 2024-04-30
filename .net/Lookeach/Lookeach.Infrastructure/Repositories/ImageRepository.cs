
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly LookeachContext _context;
        public ImageRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Image>> GetImagesByListId(List<Guid> imageId)
             => await _context.Images.Where(x => imageId.Contains(x.Id)).ToListAsync();
    }
}
