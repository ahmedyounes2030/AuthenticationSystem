using AuthenticationSystem.Application.Services;
using FluentValidation;

namespace AuthenticationSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionManager, PermissionManager>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);
        return services;
    }
}