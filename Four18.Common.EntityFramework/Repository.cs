using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Four18.Common.Repository;

#pragma warning disable 8618 // CS8618
#pragma warning disable CA1005 // Avoid excessive parameters on generic types
#pragma warning disable CA1724 // Conflict

namespace Four18.Common.EntityFramework;
#pragma warning disable CA1005 // Avoid excessive parameters on generic types
public abstract class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
    where TEntity : class, new()
    where TContext : DbContext
{
    protected TContext Context { get; }
    protected DbSet<TEntity> DbSet { get; set; }

    protected Repository(TContext context)
    {
        Context = context;
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var result = DbSet.Add(entity);
        await Context.SaveChangesAsync();
        return result.Entity;
    }

    public virtual async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities)
    {
        var results = entities.Select(entity => DbSet.Add(entity)).ToList();
        await Context.SaveChangesAsync();
        return results.Select(e => e.Entity);
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity entity, TKey id)
    {
        var old = await GetByIdAsync(id);
        if (old != null)
        {
            Context.Entry(old).State = EntityState.Detached;

            var updated = Context.Entry(entity);
            updated.State = EntityState.Modified;

            await Context.SaveChangesAsync();
            return updated.Entity;
        }

        return null;
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
    {
        var updatedEntities = new List<TEntity>();

        //should not be in local cache ideally, but just in case
        //avoiding N+1 problem of using single update approach
        //by detaching all for now.
        foreach (var old in DbSet.Local.ToList())
        {
            Context.Entry(old).State = EntityState.Detached;
        }

        foreach (var entity in entities)
        {
            var updated = Context.Entry(entity);
            updated.State = EntityState.Modified;
            updatedEntities.Add(updated.Entity);
        }

        await Context.SaveChangesAsync();
        return updatedEntities;
    }

    public virtual async Task RemoveAsync(TKey id)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) {return;}
        DbSet.Remove(existing);
        await Context.SaveChangesAsync();
    }
}

#pragma warning restore 8618 // CS8618