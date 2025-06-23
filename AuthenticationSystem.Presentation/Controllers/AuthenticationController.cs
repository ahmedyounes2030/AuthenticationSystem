global using AuthenticationSystem.Application.Interfaces;
using AuthenticationSystem.Application.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using static AuthenticationSystem.Presentation.Routes.ApiRoutes;

namespace AuthenticationSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService _authenticationService) : ControllerBase
    {
        [HttpPost(AuthenticationRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.Login(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return Unauthorized(response.Message);
        }

        [HttpPost(AuthenticationRoutes.RegisterUser)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {

            var response = await _authenticationService.RegisterUserAsync(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Message);
        }

        [HttpPost(AuthenticationRoutes.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authenticationService.RefreshTokenAsync(request);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return Unauthorized(response.Message);
        }
        [HttpPost(AuthenticationRoutes.RevokeToken)]
        public async Task<IActionResult> RevokeToke([FromBody] RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.RevokeRefreshTokenAsync(request, cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return NotFound(response.Message);
        }

    }
}
