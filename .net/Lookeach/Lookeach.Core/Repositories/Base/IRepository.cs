using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lookeach.Core.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T item);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string include = null!);

        IQueryable<T> GetAll();

        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null!, string include = null!, int? pageIndex = null,
          int? pageSize = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T t);
        void UpdateRange(IEnumerable<T> t);
    }
}
