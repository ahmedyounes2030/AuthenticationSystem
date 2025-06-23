namespace AuthenticationSystem.Application.Interfaces;

public interface IRoleService
{
    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteRoleAsync(DeleteRoleRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> AssignRoleToUserAsync(AssignRoleToUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> RemoveRoleFromUserAsync(RemoveRoleRequest request, CancellationToken cancellationToken = default);
    Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken = default);
    Task<RoleResponseWithPermissions> GetAsync(string role, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RoleResponseWithPermissions>>> GetAllAsync(CancellationToken cancellationToken = default);
}