using Lookeach.Core.Repositories.Base;
using Lookeach.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lookeach.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LookeachContext _context;
        private readonly DbSet<T> _entity;

        public Repository(LookeachContext appDbContext)
        {
            _context = appDbContext;
            _entity = _context.Set<T>();
        }

        public async Task AddAsync(T item)
        {
            await _entity.AddAsync(item);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string include = null!)
        {
            IQueryable<T> query = _entity;
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }
            return (await query.FirstOrDefaultAsync(predicate))!;
        }

        public IQueryable<T> GetAll()
        {
            return _entity;
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null!, string include = null!, int? pageIndex = null,
            int? pageSize = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!)
        {
            IQueryable<T> query = _entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(index * size).Take(size);
            }
            if (orderBy != null!)
            {
                query = orderBy(query);
            }
            return query;
        }

        public void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
            _entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entity.RemoveRange(entities);
        }

        public void Update(T t)
        {
            _entity.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> t)
        {
            _entity.AttachRange(t);
            foreach (var item in t)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
