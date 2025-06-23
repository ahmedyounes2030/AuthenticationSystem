namespace AuthenticationSystem.Application.Interfaces;

public interface IAccessTokenService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken);

    string? GetEmailClaim(ClaimsPrincipal principal);

    string? GetUsernameClaim(ClaimsPrincipal principal);

    string? GetClaim(ClaimsPrincipal principal,string claimType);
}
