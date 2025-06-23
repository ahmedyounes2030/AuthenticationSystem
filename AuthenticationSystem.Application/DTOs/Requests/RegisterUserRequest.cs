namespace AuthenticationSystem.Application.DTOs.Requests;

public record RegisterUserRequest(string userName,string password,string email);

public record LoginRequest(string email,string password);

public record RevokeRefreshTokenRequest(string refreshToken);

public record RefreshTokenRequest(string accessToken,string refreshToken);
public record ChangePasswordRequest(string Email, string CurrentPassword, string NewPassword);

public record AssignRoleToUserRequest(string userName,string role);

public record CreateRoleRequest(string name);

public record DeleteRoleRequest(string Role);
public record UpdateRoleRequest(string RoleToUpdate, string NewValue);
public record RemoveRoleRequest(string Username, string Role);
public record GrantPermissionRequest(int PermissionId, int RoleId);