namespace AuthenticationSystem.Infrastructure.Authentication;

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository, 
        IDateTimeProvider dateTimeProvider, 
        IOptions<JwtSettings> options)
    {

        _refreshTokenRepository = refreshTokenRepository;
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = options.Value;
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        var expires = _dateTimeProvider.DateTime.AddMinutes(_jwtSettings.RefreshTokenExpirationInMinutes);

        string generatedRefreshTokenValue = GenerateRefreshTokenInternal();

        var newRefreshToken = RefreshToken.Create(generatedRefreshTokenValue, userId, _dateTimeProvider.DateTime, expires);

        _refreshTokenRepository.Insert(newRefreshToken);

        return newRefreshToken;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.
          GetAsync(refreshToken, cancellationToken);
    }

    public async Task RevokeAllRefreshTokensForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.
           RevokeAllRefreshTokensForUserAsync(userId, cancellationToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenToRevoke = await _refreshTokenRepository.GetAsync(refreshToken);

        if (tokenToRevoke != null)
        {
            tokenToRevoke.Revoke();
        }

        throw new Exception("token not found.");
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrEmpty(refreshToken))
        {
            return false;
        }

        return await _refreshTokenRepository.
            IsValidRefreshToken(refreshToken, cancellationToken);
    }
    public void Delete(RefreshToken refreshToken)
    {
        _refreshTokenRepository.Remove(refreshToken);
    }
    private static string GenerateRefreshTokenInternal()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }
}
