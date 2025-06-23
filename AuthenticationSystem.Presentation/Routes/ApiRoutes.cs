namespace AuthenticationSystem.Presentation.Routes;

public static class ApiRoutes
{
    public static class AuthenticationRoutes
    {
        private const string Base = "api/authentication";

        public const string Login = $"{Base}/login";
        public const string RegisterUser = $"{Base}/register-user";
        public const string RefreshToken = $"{Base}/refresh-token";
        public const string RevokeToken = $"{Base}/revoke-token";
    }

    public static class PermissionRoutes
    {
        private const string Base = "api/permission";

        public const string GrantToRole = $"{Base}/grant-permission-to-role";
        public const string RevokeFromRole = $"{Base}/revoke-permission-from-role";
        public const string GetAll = $"{Base}/get-all";
    }

    public static class RoleRoutes
    {
        private const string Base = "api/role";

        public const string Create = $"{Base}/create-role";
        public const string GetByName = $"{Base}/get-role-by-name/{{name}}";
        public const string GetAll = $"{Base}/get-all";
        public const string AssignToUser = $"{Base}/assign-role-to-user";
        public const string RemoveFromUser = $"{Base}/remove-role-from-user";
    }

    public static class UserRoutes
    {
        private const string Base = "api/user";

        public const string GetById = $"{Base}/get-user-by-id/{{id}}";
        public const string GetByEmail = $"{Base}/get-by-email/{{email}}";
        public const string GetByUsername = $"{Base}/get-by-username/{{username}}";
        public const string ChangePassword = $"{Base}/change-password";
        public const string Delete = $"{Base}/delete/{{username}}";
        public const string GetAll = $"{Base}/get-all";
    }

}
