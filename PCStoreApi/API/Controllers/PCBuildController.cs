using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PCStoreApi.Application.DTOs.PCBuild;
using PCStoreApi.Application.Interfaces;
using System.Collections;

namespace PCStoreApi.API.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var builds = await _pcBuildService.GetAllBuildsAsync();
            return Ok(builds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var build = await _pcBuildService.GetBuildByIdAsync(id);
            return build is null ? NotFound() : Ok(build);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PCBuildUpdateDto dto)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _pcBuildService.DeleteBuildAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
