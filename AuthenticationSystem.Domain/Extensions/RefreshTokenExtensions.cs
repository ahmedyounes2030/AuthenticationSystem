using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Domain.Extensions;

public static class RefreshTokenExtensions
{
    public static bool IsRevokedOrExpired(this RefreshToken token)
    {
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        return token.IsRevoked || DateTime.UtcNow < token.RevokedAt;
    }
}