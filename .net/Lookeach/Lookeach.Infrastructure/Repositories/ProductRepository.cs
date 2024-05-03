using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly LookeachContext _context;
        public ProductRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Product>> GetProductBtSizeId(List<Guid> sizes, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            if (query.Select(x => x.Sizes).Any(x => x != null!))
            {
                query = query.Where(x => x.Sizes!.Any(x => sizes.Contains(x.Id)));
            }
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        private void Include(ref IQueryable<Product> products, string? includes = null!)
        {
            if (includes != "None")
            {
                if (includes != null!)
                {
                    foreach (var item in includes.Split(","))
                    {
                        products = products.Include(item);
                    }
                }
                else
                {
                    products = products.Include(x => x.Sizes).Include(x => x.Categories).Include(x => x.Colors);
                }
            }
            if (products.Select(x => x.ProductImages).Any(x => x != null!))
            {
                products = products.AsNoTracking().Include(x => x.ProductImages)!.ThenInclude(x => x.Image);
            }
        }

        private void Pagination(ref IQueryable<Product> products, int? pageIndex = null, int? pageSize = null)
        {
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                products = products.Skip(index * size).Take(size);
            }
        }

        public async Task<List<Product>> GetProductByBrandId(Guid id, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            query = query.Where(x => x.BrandId == id);
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetProductByBrandIds(List<Guid> brands, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            query = query.Where(x => brands.Contains(x.BrandId));
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategoryId(List<Guid> categoryIds, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            query = query.Where(x => x.Categories.Any(x => categoryIds.Contains(x.Id)));
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetProductByColorIds(List<Guid> colors, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            if (query.Select(x => x.Colors).Any(x => x != null!))
            {
                query = query.Where(x => x.Colors!.Any(x => colors.Contains(x.Id)));
            }
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id, string? include = null)
        {
            IQueryable<Product> query = _context.Products;
            query = query.Where(x => x.Id == id);
            if (query != null && query.Count() > 0)
            {
                Include(ref query, include);
                return (await query.FirstOrDefaultAsync())!;
            }
            return null!;
        }

        public async Task<List<Product>> GetProducts(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (quantity != null)
            {
                query = query.Where(x => x.Quantity >= quantity);
            }
            if (priceFrom.HasValue)
            {
                query = query.Where(x => x.Price >= priceFrom);
            }
            if (priceTo.HasValue)
            {
                query = query.Where(x => x.Price <= priceTo);
            }
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetProducts(DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Product> query = _context.Products;
            if (dateFrom.HasValue)
            {
                query = query.Where(x => x.CreatedOn >= dateFrom);
            }
            if (dateTo.HasValue)
            {
                query = query.Where(x => x.CreatedOn <= dateTo);
            }
            Pagination(ref query, pageIndex, pageSize);
            Include(ref query, includes);
            return await query.ToListAsync();
        }
    }
}
