using Microsoft.AspNetCore.Mvc;
using PCStoreApi.Application.DTOs.Auth;
using PCStoreApi.Application.Interfaces;

namespace PCStoreApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("Email already exists.");
            }

            return Ok(new UserResponseDto
            {
               Id = user.Id,
               Email = user.Email,
               Role = user.Role,
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response == null)
            {
                return BadRequest("Invalid username or Password");
            }

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);

            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token");
            } 

            return Ok(result);
        }
    }
}
