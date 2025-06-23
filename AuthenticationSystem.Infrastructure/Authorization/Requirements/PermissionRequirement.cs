namespace AuthenticationSystem.Infrastructure.Authorization.Requirements;
//Purpose: Serves as a container for authorization requirements
public class PermissionRequirement : IAuthorizationRequirement
{
    public Permissions Permission { get; }

    public PermissionRequirement(Permissions permission)
    {
        Permission = permission;
    }
}

/*
 Why This Design?

Type Safety: Ensures requirements always contain valid permissions

Immutability: Prevents requirement modification during authorization

Framework Integration: Native compatibility with ASP.NET Core pipeline
 */