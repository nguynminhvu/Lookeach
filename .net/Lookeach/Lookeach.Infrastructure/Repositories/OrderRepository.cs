using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly LookeachContext _context;
        public OrderRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Order>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null, int? pageSize = null, string? includes = null!)
        {
            IQueryable<Order> query = _context.Orders;
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

        private void Pagination(ref IQueryable<Order> orders, int? pageIndex = null, int? pageSize = null)
        {
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                orders = orders.Skip(index * size).Take(size);
            }
        }

        private void Include(ref IQueryable<Order> orders, string? includes = null!)
        {
            if (includes != "None")
            {
                if (includes != null!)
                {
                    foreach (var item in includes.Split(","))
                    {
                        orders = orders.Include(item);
                    }
                }
                else
                {
                    orders = orders.Include(x => x.OrderItems).Include(x => x.PaymentMethod).Include(x => x.Vouchers);
                }
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(Guid userId, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Order> query = _context.Orders;
            query = query.Where(x => x.UserId == userId);
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
