using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCStoreApi.Application.DTOs.User;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace PCStoreApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserCreateDto> _createValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;

        public UserController(IUserService userService, IValidator<UserCreateDto> createValidator, IValidator<UserUpdateDto> updateValidator)
        {
            _userService = userService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto) 
        {
            var result = await _createValidator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());
               
                return BadRequest(new { errors });
            }

            var userIdcClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdcClaim is null) 
                return Unauthorized();

            var userId = Guid.Parse(userIdcClaim);

            var created = await _userService.CreateUserAsync(userId, dto);

            return CreatedAtAction(nameof(GetById),
                new { id = created.UserId },
                created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserUpdateDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage.ToArray()));

                return BadRequest(new { errors });
            }

            var success = await _userService.UpdateUserAsync(id, dto);

            return success ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
