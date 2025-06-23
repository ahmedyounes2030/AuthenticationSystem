using AuthenticationSystem.Application.DTOs.Requests;
using AuthenticationSystem.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AuthenticationSystem.Presentation.Routes.ApiRoutes;

namespace AuthenticationSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HasPermission(Permissions.CanRead)]
        [HttpGet(UserRoutes.GetById)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            return user is null ? NotFound("user not found.") : Ok(user);
        }

        [HasPermission(Permissions.CanRead)]
        [HttpGet(UserRoutes.GetByEmail)]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("email cannot be empty.");
            }

            var user = await _userService.GetByEmailAsync(email, cancellationToken);

            return user is null ? NotFound("user not found.") : Ok(user);
        }

        [HasPermission(Permissions.CanRead)]
        [HttpGet(UserRoutes.GetByUsername)]
        public async Task<IActionResult> GetByUsername(string username, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("username cannot by empty.");
            }

            var user = await _userService.GetByUsernameAsync(username, cancellationToken);

            return user is null ? NotFound("user not found.") : Ok(user);
        }


        [HasPermission(Permissions.CanUpdate)]
        [HttpPut(UserRoutes.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.ChangePasswordAsync(request, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Message);
        }

        [HasPermission(Permissions.CanDelete)]
        [HttpDelete(UserRoutes.Delete)]

        public async Task<IActionResult> DeleteUser(string username, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest(new { Error = "username cannot be empty." });

            var response = await _userService.DeleteUserAsync(username, cancellationToken);

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HasPermission(Permissions.CanRead)]
        [HttpGet(UserRoutes.GetAll)]

        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersAsync(cancellationToken);

            return Ok(result);
        }


    }

}
