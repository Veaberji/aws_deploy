using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);
}