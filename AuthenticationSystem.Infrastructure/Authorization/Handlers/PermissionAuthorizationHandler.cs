namespace AuthenticationSystem.Infrastructure.Authorization.Handlers;
//Purpose: Core logic for permission validation
internal sealed class PermissionAuthorizationHandler
    (IServiceScopeFactory _serviceScopeFactory) 
    : AuthorizationHandler<PermissionRequirement>
{    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 1. Extract permissions claim from JWT
        var combinedPermissionsClaims = context.User.Claims
            .FirstOrDefault(x => x.Type == CustomClaims.Permissions)?.Value;

        // 2. Parse claim value to long (returns 0 if invalid)
        if (!long.TryParse(combinedPermissionsClaims, out long parsedPermissionsValue))
            return Task.CompletedTask;

        // 3. Convert to Permissions enum
        var userPermissions = (Permissions)parsedPermissionsValue;

        // 4. Create service scope for dependency resolution
        using var scope = _serviceScopeFactory.CreateScope();
        var permissionManager = scope.ServiceProvider.GetRequiredService<IPermissionManager>();

        // 5. Check permissions using bitwise operations
        if (permissionManager.HasPermission(userPermissions, requirement.Permission))
        {
            context.Succeed(requirement); // Grant access
        }

        return Task.CompletedTask;
    }
}