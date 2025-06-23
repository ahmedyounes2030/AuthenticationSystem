namespace AuthenticationSystem.Application.Services;

public class UserService(
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork,
    IPasswordHasher _passwordHasher,
    IPermissionManager _permissionManager
    ) : IUserService
{
    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }

    public async Task<UserResponse?> GetByUsernameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(userName, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }

    public async Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(x => x.Name).ToList();

        return new UserResponse(user.Id, user.Username, user.Email, roles);
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        if (users is null)
        {
            return Enumerable.Empty<UserResponse>();
        }


        return users.Select(x => new UserResponse(x.Id, x.Username, x.Email,
            x.Roles.Select(r => r.Name).ToList()));
    }

    public async Task<Result<string>> DeleteUserAsync(string username, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username, cancellationToken);

        if (existingUser is null)
        {
            return Result<string>.Failure($"user with username '{username}' does not exist.");
        }
        var isAdmin = existingUser.Roles
                    .Any(x => _permissionManager.IsAdmin(x.Permissions));

        if (isAdmin)
        {
            return Result<string>.Failure(ErrorMessages.CannotDeleteAdmin);
        }

        _userRepository.Delete(existingUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"user with username '{username}' has been deleted.");
    }
    public async Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure($"no user found with email '{request.Email}'");
        }

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return Result<string>.Failure(ErrorMessages.PasswordMismatch);
        }

        string newHashPassword = _passwordHasher.Hash(request.NewPassword);

        user.SetPassword(newHashPassword);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(SuccessMessages.PasswordChangedSuccessfully);
    }
}