namespace AuthenticationSystem.Application.Services;

public class PermissionService(
    IPermissionRepository _permissionRepository,
    IRoleRepository _roleRepository,
    IUnitOfWork _unitOfWork) : IPermissionService
{
    public async Task<Result<PermissionResponse>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetPermissionByNameAsync(name, cancellationToken);
        if (permission is null)
        {
            return Result<PermissionResponse>.Failure("permission not found.");
        }

        var response = new PermissionResponse(permission.Id, permission.Name);

        return Result<PermissionResponse>.Success(response);
    }
    public async Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await _permissionRepository.GetAllPermissionsAsync(cancellationToken);

        return permissions.Select(p => new PermissionResponse(p.Id, p.Name));
    }

    public async Task<Result<string>> GrantPermissionToRoleAsync(GrantPermissionRequest request, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);
        if (permission is null)
        {
            return Result<string>.Failure($"permission with id '{request}' not found.");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            return Result<string>.Failure($"role with id '{request.RoleId}' not found.");
        }

        var permissionToGrant = ((Permissions)permission.Value);

        role.GrantPermission(permissionToGrant);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"Permission '{permission.Name}' assigned to role '{role.Name}' successfully.");
    }

    public async Task<Result<string>> RevokePermissionFromRoleAsync(int permissionId, int roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return Result<string>.Failure($"role with id '{roleId}' not found.");
        }

        var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return Result<string>.Failure($"permission with id '{permissionId}' not found.");
        }

        var permissionToRevoke = ((Permissions)permission.Value);

        role.RevokePermission(permissionToRevoke);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"permission '{permission.Name}' removed from role '{role.Name}' successfully.");
    }
}
