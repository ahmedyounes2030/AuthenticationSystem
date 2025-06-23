namespace AuthenticationSystem.Application.Services;

internal class PermissionManager : IPermissionManager
{
    public Permissions GrantPermission(Permissions existingPermissions, Permissions permissionsToGrant)
    {
        return existingPermissions | permissionsToGrant;
    }

    public bool HasPermission(Permissions userPermissions, Permissions requiredPermissions)
    {
        return (userPermissions & requiredPermissions) == requiredPermissions;
    }
    public Permissions RevokePermission(Permissions existingPermissions, Permissions permissionsToRevoke)
    {
        return existingPermissions & ~permissionsToRevoke;
    }
    public bool IsAdmin(Permissions permissions)
    {
        return HasPermission(permissions,
            Permissions.CanAdd | Permissions.CanRead | Permissions.CanUpdate | Permissions.CanDelete);

        //return HasPermission(permissions, Permissions.All);
    }
}