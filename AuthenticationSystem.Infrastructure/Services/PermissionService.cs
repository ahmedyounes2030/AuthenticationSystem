namespace AuthenticationSystem.Infrastructure.Services;

internal sealed class PermissionService : IPermissionService
{
    public HashSet<string> GetPermissions()
    {
        return Enum.GetNames<Permissions>().ToHashSet();
    }
}
