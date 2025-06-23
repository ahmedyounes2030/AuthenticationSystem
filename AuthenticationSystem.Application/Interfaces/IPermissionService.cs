namespace AuthenticationSystem.Application.Interfaces;

public interface IPermissionService
{
    Task<Result<PermissionResponse>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<Result<string>> GrantPermissionToRoleAsync(GrantPermissionRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> RevokePermissionFromRoleAsync(int permissionId, int roleId, CancellationToken cancellationToken = default);
}
