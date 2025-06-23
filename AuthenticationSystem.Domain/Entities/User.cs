namespace AuthenticationSystem.Domain.Entities;

public class User:Entity<int>
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool EmailVerified { get; private set; }
    private List<RefreshToken> _refreshTokens = new List<RefreshToken>();
    private List<Role> _roles = new List<Role>();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens;
    public IReadOnlyList<Role> Roles => _roles;

    private User(string username, string passwordHash, string email)
    {
        this.Username = username;
        this.PasswordHash = passwordHash;
        this.Email = email;
    }

    public static User Create(string username, string password, string email) =>
                             new User(username, password, email);

    public void SetPassword(string password)
    {
        this.PasswordHash = password;
    }

    public void SetEmail(string email)
    {
        this.Email = email;
    }

    public void SetUsername(string username)
    {
        this.Username = username;
    }

    public void SetEmailVerified(bool verified)
    {
        this.EmailVerified = verified;
    }

    public void AssignRole(Role role)
    {
        if (_roles.Contains(role))
            return;

        _roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        if (_roles.Contains(role))
        {
            _roles.Remove(role);
        }
    }
}
