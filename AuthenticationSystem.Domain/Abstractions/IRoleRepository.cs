using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Domain.Abstractions;

public interface IRoleRepository : IRepository<int, Role>
{
    Task<Role?> GetRoleAsync(string role, CancellationToken cancellationToken = default);

    Task<bool> IsRoleExists(string role, CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
}
