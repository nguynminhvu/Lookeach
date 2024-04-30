using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Infrastructure.Repositories
{
    public class CategoryImageRepository : Repository<CategoryImage>, ICategoryImageRepository
    {
        private readonly LookeachContext _context;
        public CategoryImageRepository(LookeachContext appDbContext) : base(appDbContext)
        {

            _context=appDbContext;
        }

        public async Task<List<CategoryImage>> GetBrandImagesByCategoryId(Guid categoryId)
           => await _context.CategoryImages.Where(x => x.CategoryId == categoryId).ToListAsync();
    }
}
