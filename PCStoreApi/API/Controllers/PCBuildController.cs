using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCStoreApi.Application.DTOs.PCBuild;
using PCStoreApi.Application.Interfaces;

namespace PCStoreApi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PCBuildController : ControllerBase
    {
        private readonly IPCBuildService _pcBuildService;
        private readonly IUserService _userService;
        private readonly IValidator<PCBuildCreateDto> _createValidator;
        private readonly IValidator<PCBuildUpdateDto> _updateValidator;

        public PCBuildController(
            IPCBuildService pCBuildService,
            IUserService userService,
            IValidator<PCBuildCreateDto> createValidator,
            IValidator<PCBuildUpdateDto> updateValidator)
        {
            _pcBuildService = pCBuildService;
            _userService = userService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var builds = await _pcBuildService.GetAllBuildsAsync();
            return Ok(builds);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var build = await _pcBuildService.GetBuildByIdAsync(id);
            return build is null ? NotFound() : Ok(build);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return NotFound($"No user found with ID {userId}");
            }

            var build = await _pcBuildService.GetBuildByUserIdAsync(userId);

            return build is null ? NotFound($"No build found for this user ID {userId}") : Ok(build);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PCBuildCreateDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(errors);
            }
            var created = await _pcBuildService.CreateBuildAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.PCBuildId },
              created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PCBuildUpdateDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(errors);
            }
            var success = await _pcBuildService.UpdateBuildAsync(id, dto);

            return success ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _pcBuildService.DeleteBuildAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
