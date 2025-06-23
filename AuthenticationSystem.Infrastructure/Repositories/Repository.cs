namespace AuthenticationSystem.Infrastructure.Repositories;

public class Repository<TPrimaryKey, TEntity> :
    IRepository<TPrimaryKey, TEntity> where TEntity : Entity<TPrimaryKey>
{
    protected ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext context) =>
        _dbContext = context;

    public virtual void Insert(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }
    public virtual void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TPrimaryKey key, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(key, cancellationToken);
    }

}