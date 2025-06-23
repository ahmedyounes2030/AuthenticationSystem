namespace AuthenticationSystem.Application.Interfaces;
public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetByUsernameAsync(string userName, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteUserAsync(string username, CancellationToken cancellationToken = default);
}