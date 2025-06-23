namespace AuthenticationSystem.Application.DTOs.Responses;

public record UserResponse(int id,string userName,string email,List<string>roles);

public record LoginResponse(string accessToken,string refreshToken);

public record RefreshTokenResponse(string accessToken,string refreshToken);

public record RoleResponse(int id,string name);
public record RoleResponseWithPermissions(int Id, string Role, string[] Permissions);

public record PermissionResponse(int Id, string Name);
