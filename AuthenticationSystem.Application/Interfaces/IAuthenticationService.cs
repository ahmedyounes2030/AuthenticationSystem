namespace AuthenticationSystem.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<UserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default);
}
