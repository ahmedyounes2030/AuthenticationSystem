namespace AuthenticationSystem.Infrastructure.Repositories;

internal class RoleRepository(ApplicationDbContext _dbContext) : IRoleRepository
{
    public void Insert(Role entity)
    {
        _dbContext.Set<Role>().Add(entity);
    }

    public void Update(Role entity)
    {
        _dbContext.Set<Role>().Update(entity);
    }

    public void Delete(Role entity)
    {
        _dbContext.Set<Role>().Add(entity);  
    }

    public async Task<bool> IsRoleExists(string role, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .AnyAsync(x => x.Name == role, cancellationToken);
    }

    public async Task<Role?> GetRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .FirstOrDefaultAsync(x => x.Name == role, cancellationToken);
    }


    public async Task<Role?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .AsNoTracking()
                               .ToListAsync(cancellationToken);
    }
}