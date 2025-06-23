using AuthenticationSystem.Application.DTOs.Requests;
using AuthenticationSystem.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AuthenticationSystem.Presentation.Routes.ApiRoutes;

namespace AuthenticationSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HasPermission(Permissions.CanUpdate)]
        [HttpPut(PermissionRoutes.GrantToRole)]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] GrantPermissionRequest request, CancellationToken cancellationToken)
        {
            var response = await _permissionService.GrantPermissionToRoleAsync(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Message);
        }

        [HasPermission(Permissions.CanUpdate)]
        [HttpPut(PermissionRoutes.RevokeFromRole)]
        public async Task<IActionResult> RevokePermissionFromRole(int permissionId, int roleId, CancellationToken cancellationToken)
        {
            var response = await _permissionService.RevokePermissionFromRoleAsync(permissionId, roleId, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Message);
        }

        [HasPermission(Permissions.CanRead)]
        [HttpGet(PermissionRoutes.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _permissionService.GetAllPermissionsAsync(cancellationToken);

            return response is not null ? Ok(response) : NotFound();
        }

    }

}
