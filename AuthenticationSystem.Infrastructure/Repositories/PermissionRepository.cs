namespace AuthenticationSystem.Infrastructure.Repositories;

internal class PermissionRepository(ApplicationDbContext _dbContext) : IPermissionRepository
{
    public void Insert(Permission entity)
    {
        _dbContext.Set<Permission>().Add(entity);
    }

    public void Update(Permission entity)
    {
        _dbContext.Set<Permission>().Update(entity);
    }
    public void Delete(Permission entity)
    {
        _dbContext.Set<Permission>().Remove(entity);
    }

    public async Task<bool> IsPermissionExistsAsync(string permission, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Permission>()
                               .AnyAsync(x => x.Name == permission, cancellationToken);
    }

    public async Task<Permission?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return await
               _dbContext.Set<Permission>()
                         .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public async Task<Permission?> GetPermissionByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await
              _dbContext.Set<Permission>()
                        .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Permission>()
                               .ToListAsync(cancellationToken);
    }
}