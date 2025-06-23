namespace AuthenticationSystem.Infrastructure.Authorization.Handlers;
//Purpose: Dynamically generates authorization policies
//Extends DefaultAuthorizationPolicyProvider
//Maintains all built-in policy functionality
//Overrides only policy resolution behavior
internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
        : base(options) {  }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // 1. Check built-in policies first
        var policy = await base.GetPolicyAsync(policyName);

        // 2. Create dynamic policy for permission-based names
        if (policy is null && PolicyHelper.IsValidPolicyName(policyName))
        {
            // Extract permission from policy name (e.g., "permissions7")
            var requiredPermission = PolicyHelper.GetPolicyPermissions(policyName);

            // Build policy with PermissionRequirement
            policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(requiredPermission))
                .Build();
        }
        return policy;
    }
}