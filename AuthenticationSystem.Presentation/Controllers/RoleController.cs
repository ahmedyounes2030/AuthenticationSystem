using AuthenticationSystem.Application.DTOs.Requests;
using AuthenticationSystem.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AuthenticationSystem.Presentation.Routes.ApiRoutes;

namespace AuthenticationSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRoleService _roleService) : ControllerBase
    {
        [HasPermission(Permissions.CanAdd)]
        [HttpPost(RoleRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _roleService.CreateRoleAsync(request, cancellationToken);

            return Ok(response);
        }
        [HasPermission(Permissions.CanRead)]
        [HttpGet(RoleRoutes.GetByName)]
        public async Task<IActionResult> GetRoleByName(string name, CancellationToken cancellationToken)
        {
            var response = await _roleService.GetAsync(name);

            return response is not null ? Ok(response) : NotFound("role not found");
        }


        [HasPermission(Permissions.CanRead)]
        [HttpGet(RoleRoutes.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _roleService.GetAllAsync(cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return NotFound(response.Message);
        }

        [HasPermission(Permissions.CanUpdate)]
        [HttpPost(RoleRoutes.RemoveFromUser)]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _roleService.RemoveRoleFromUserAsync(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Message);
        }

        [HasPermission(Permissions.CanUpdate)]
        [HttpPost(RoleRoutes.AssignToUser)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _roleService.AssignRoleToUserAsync(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Message);
        }
    }
}
