namespace AuthenticationSystem.Application.Services;

public class RoleService(
    IRoleRepository _rolesRepository,
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork) : IRoleService
{
    private const byte MinimumAllowedRoles = 1;
    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        if(await _rolesRepository.IsRoleExists(request.name))
        {
            throw new Exception("the role is already exists.");
        }

        var role = new Role(request.name);
        _rolesRepository.Insert(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoleResponse(role.Id, role.Name);
    }
    public async Task<Result<string>> AssignRoleToUserAsync(AssignRoleToUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.userName);
        if(user is null)
        {
            return Result<string>.Failure($"the user '{request.userName}' not found.");
        }

        var role = await _rolesRepository.GetRoleAsync(request.role, cancellationToken);
        if (role is null)
        {
            return Result<string>.Failure($"the role '{request.role}' was not found.");
        }

        user.AssignRole(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"Role '{request.role}' assigned to user '{request.userName}' successfully.");
    }

    public async Task<Result<string>> DeleteRoleAsync(DeleteRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _rolesRepository.GetRoleAsync(request.Role);
        if(role is null)
        {
            return Result<string>.Failure($"role '{request.Role} not found.'");
        }

        _rolesRepository.Delete(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"role has been deleted successfully.");
    }

    public async Task<Result<string>> RemoveRoleFromUserAsync(RemoveRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if(user is null)
        {
            return Result<string>.Failure($"user '{request.Username}' not found.");
        }

        if(user.Roles.Count == MinimumAllowedRoles)
        {
            return Result<string>.Failure(ErrorMessages.RoleRequired);
        }

        var roleToRemove = await _rolesRepository.GetRoleAsync(request.Role,cancellationToken);
        if(roleToRemove is null)
        {
            return Result<string>.Failure($"Role '{request.Role}' not found.");
        }

        user.RemoveRole(roleToRemove);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"Role '{request.Role}' removed from user '{request.Username}' successfully.");
    }
    public async Task<Result<string>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _rolesRepository.GetRoleAsync(request.RoleToUpdate);
        if(role is null)
        {
            return Result<string>.Failure($"role {request.RoleToUpdate} not found.");
        }

        role.SetName(request.NewValue);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("role updated successfully.");
    }

    public async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _rolesRepository.IsRoleExists(role, cancellationToken);
    }
    public async Task<RoleResponseWithPermissions> GetAsync(string role, CancellationToken cancellationToken = default)
    {
        var _role = await _rolesRepository.GetRoleAsync(role,cancellationToken);
        if(_role is not null)
        {
            var permissions = _role.Permissions.ToStringArray();
            return new RoleResponseWithPermissions(_role.Id, _role.Name, permissions);
        }

        return null!;
    }
    public async Task<Result<IEnumerable<RoleResponseWithPermissions>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _rolesRepository.GetAllAsync(cancellationToken);
        if (roles is null)
        {
            return Result<IEnumerable<RoleResponseWithPermissions>>.Failure("not roles found.");
        }

        var roleDtos = roles.Select(x => new RoleResponseWithPermissions(x.Id, x.Name, x.Permissions.ToStringArray()));

        return Result<IEnumerable<RoleResponseWithPermissions>>.Success(roleDtos);
    }
}