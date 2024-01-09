namespace Four18.Organization.Data.Interfaces;

// TODO: move IRepository to tdrx.common nuget package
public interface IRepository<TEntity, in TKey> where TEntity : class {
    IQueryable<TEntity> GetAll();

    Task<TEntity?> GetByIdAsync(TKey id);

    Task<TEntity> AddAsync(TEntity entity);

    Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities);

    Task<TEntity?> UpdateAsync(TEntity entity, TKey id);

    Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities);

    Task RemoveAsync(TKey id);
}