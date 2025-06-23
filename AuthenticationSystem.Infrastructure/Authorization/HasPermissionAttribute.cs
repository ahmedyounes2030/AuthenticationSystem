//1
namespace AuthenticationSystem.Infrastructure.Authorization;
//Purpose: Declarative permission checks on controllers/actions.
//Core Purpose: Provides declarative interface for securing endpoints using
//permission flags while integrating with ASP.NET Core's authorization pipeline.
public class HasPermissionAttribute : AuthorizeAttribute//Triggers authorization pipeline,Supports policy/Roles/authentication schemes
{
    public HasPermissionAttribute() { }

    public HasPermissionAttribute(string policy) => Policy = policy;

    public HasPermissionAttribute(Permissions permission) => Permissions = permission;

    public Permissions Permissions
    {
        get
        {//Decode via PolicyHelper
            return !string.IsNullOrEmpty(Policy) ?
                PolicyHelper.GetPolicyPermissions(Policy) : Permissions.None;
        }
        set
        {//Encode via PolicyHelper
            Policy = value != Permissions.None ?
                PolicyHelper.GeneratePolicyNameFor(value) : string.Empty;
        }
    }
}


/*
 Architectural Integration
End-to-End Flow Example:

Developer decorates endpoint:


[HasPermission(Permissions.CanUpdate | Permissions.CanDelete)] // 4 + 8 = 12

Attribute setter converts to policy name:
Policy = PolicyHelper.GeneratePolicyNameFor(12) → "permissions12"

Authorization system requests policy "permissions12"

PolicyProvider reconstructs requirement:
PolicyHelper.GetPolicyPermissions("permissions12") → Permissions value 12

Authorization handler compares user's permissions against value 12 
 */