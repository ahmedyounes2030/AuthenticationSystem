namespace AuthenticationSystem.Infrastructure.Authorization;
//Core Purpose: Acts as a policy naming convention manager that bridges between
//human-readable permissions and ASP.NET Core's policy-based authorization system.
internal static class PolicyHelper
{
    public const string Policy_Prefix = "permissions";

    //Security Function: Validates if a policy name belongs to the permission system
    //Usage Context: Gatekeeper for dynamic policy creation
    public static bool IsValidPolicyName(string policyName)
    {
        return policyName is not null && policyName.
            StartsWith(Policy_Prefix, StringComparison.OrdinalIgnoreCase);
    }
    public static string GeneratePolicyNameFor(Permissions permissions)
    {
        return $"{Policy_Prefix}{(int)permissions}";
    }
    //Permission Reconstruction: Decodes policy names back to enum flags
    public static Permissions GetPolicyPermissions(string policyName)
    {
        var permissionsValue = int.Parse(policyName[Policy_Prefix.Length..]);

        return (Permissions)permissionsValue;
    }
}
