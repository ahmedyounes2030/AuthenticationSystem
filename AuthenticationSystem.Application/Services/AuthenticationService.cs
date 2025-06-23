namespace AuthenticationSystem.Application.Services;

public class AuthenticationService
    (
    IUserRepository _userRepository,
    IPasswordHasher _passwordHasher,
    IAccessTokenService _accessTokenService,
    IRefreshTokenService _refreshTokenService,
    IUnitOfWork _unitOfWork
    )
    : IAuthenticationService
{
    public async Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.email, cancellationToken);
        if (user is null)
        {
            return Result<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        bool verified = _passwordHasher.Verify(request.password, user.PasswordHash);
        if (!verified)
        {
            return Result<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        var accessToken = _accessTokenService.GenerateAccessToken(user);

        var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken!, refreshToken.Token));
    }
    public async Task<Result<UserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {

        if(!await _userRepository.IsEmailUniqueAsync(request.email, cancellationToken))
        {
            return Result<UserResponse>.Failure(ErrorMessages.EmailAlreadyExists);
        }

        if(!await _userRepository.IsUsernameUniqueAsync(request.userName,cancellationToken))
        {
            return Result<UserResponse>.Failure(ErrorMessages.UsernameAlreadyExists);
        }

        string passwordHash = _passwordHasher.Hash(request.password);

        var user = User.Create(request.userName, passwordHash, request.email);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserResponse>.Success(new UserResponse(user.Id, user.Username, user.Email, []));
    }
    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var storedRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.refreshToken, cancellationToken);
        if (storedRefreshToken is null)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenInvalid);
        }

        if (storedRefreshToken.IsRevokedOrExpired())
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenExpired);
        }

        var claimsPrincipal = _accessTokenService.GetPrincipalFromExpiredAccessToken(request.accessToken);
        if (claimsPrincipal is null)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.AccessTokenMalformed);
        }

        string? userName = _accessTokenService.GetUsernameClaim(claimsPrincipal);

        var user = await _userRepository.GetByUsernameAsync(userName!, cancellationToken);

        if (user!.Id != storedRefreshToken.UserId)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenUserMismatch);
        }

        _refreshTokenService.Delete(storedRefreshToken);

        string newAccessToken = _accessTokenService.GenerateAccessToken(user);

        var newRefreshToken = _refreshTokenService.GenerateRefreshToken(user!.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse(newAccessToken!, newRefreshToken.Token));
    }

    public async Task<Result<string>> RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenToBeRevoked = await _refreshTokenService.GetRefreshTokenAsync(request.refreshToken, cancellationToken);
        if (tokenToBeRevoked is null)
        {
            return Result<string>.Failure(ErrorMessages.RefreshTokenNotFound);
        }

        tokenToBeRevoked.Revoke();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(SuccessMessages.RevokeRefreshTokenSuccess);
    }
}