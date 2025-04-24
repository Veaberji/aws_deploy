using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MusiciansAPP.DAL.DBDataProvider;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected Repository(DbContext context, IDataLogger<IUnitOfWork> logger)
    {
        Context = context;
        Logger = logger;
    }

    protected DbContext Context { get; private init; }

    protected IDataLogger<IUnitOfWork> Logger { get; private init; }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
    }

    protected bool IsNewItem(IEnumerable<string> existingNames, string name)
    {
        return !existingNames.Contains(name);
    }
}