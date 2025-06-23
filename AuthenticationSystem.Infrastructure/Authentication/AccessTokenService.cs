namespace AuthenticationSystem.Infrastructure.Authentication;

internal sealed class AccessTokenService : IAccessTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AccessTokenService(
        IOptions<JwtSettings> jwtSettings,
        IOptions<JwtBearerOptions> jwtBearerOptions,
        IDateTimeProvider dateTimeProvider)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenValidationParameters = jwtBearerOptions.Value.TokenValidationParameters;
    }
    public string GenerateAccessToken(User user)
    {
        string key = _jwtSettings.SecretKey;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

        var issuedAt = DateTime.UtcNow;
        var issuedAtUnix = ((DateTimeOffset)issuedAt).ToUnixTimeSeconds();
        var expiresAt = issuedAt.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

        List<Claim> claims =
        [
            new Claim(CustomClaims.Email,user.Email),
            new Claim(CustomClaims.Username,user.Username),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,issuedAtUnix.ToString()),
        ];

        if (user.Roles.Any())
        {
            long combinedPermissions = 0;

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
                //Collects all permissions from the user's roles
                combinedPermissions |= (long)role.Permissions;
            }
            //Encodes the permissions into a single integer (bitmask) and includes it in a claim in the JWT token.
            claims.Add(new Claim(CustomClaims.Permissions, combinedPermissions.ToString()));
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Expires = expiresAt,
            NotBefore = issuedAt,
            SigningCredentials = credentials,
        };

        return new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler().CreateToken(descriptor);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken)
    {
        var tokenValidationParameters = _tokenValidationParameters;
        tokenValidationParameters.ValidateLifetime = false; // Disable lifetime validation for expired tokens
        return ValidateAccessToken(accessToken, tokenValidationParameters);
    }
    private ClaimsPrincipal ValidateAccessToken(string accessToken, TokenValidationParameters tokenValidationParameters)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = jwtHandler.ValidateToken(accessToken, tokenValidationParameters,
              out SecurityToken validatedToken);

        if (!IsTokenHasValidSecurityAlgorithm(validatedToken))
        {
            throw new InvalidAccessTokenException();
        }

        return claimsPrincipal;
    }

    private static bool IsTokenHasValidSecurityAlgorithm(SecurityToken token)
    {
        return
              token is JwtSecurityToken jwtToken &&
              jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase);
    }

    public string? GetClaim(ClaimsPrincipal principal, string claimType)
    {
        return principal.Claims
              .FirstOrDefault(c => c.Type == claimType)?.Value.Trim();
    }

    public string? GetEmailClaim(ClaimsPrincipal principal)
    {
        return GetClaim(principal, CustomClaims.Email);
    }
    public string? GetUsernameClaim(ClaimsPrincipal principal)
    {
        return GetClaim(principal, CustomClaims.Username);
    }
}
