using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wedev.Shared.Global;
using wedev.Service.Services;

namespace wedev.WebApi.Controllers.Global
{
    [ApiController]
    [Route("Global/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GlobalServices _globalServices;

        public UserController(GlobalServices globalServices)
        {
            _globalServices = globalServices;
        }

        [HttpGet("{tenantId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(Guid tenantId)
        {
            return Ok(await _globalServices.GetAllUsersByTenantAsync(tenantId));
        }

        [HttpGet("{tenantId}/{userId}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(Guid tenantId, Guid userId)
        {
            var user = await _globalServices.GetUserByIdAsync(userId, tenantId);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost("{tenantId}")]
        [Authorize]
        public async Task<IActionResult> CreateUser(Guid tenantId, [FromBody] UserDto userDto)
        {
            var createdUser = await _globalServices.CreateUserAsync(userDto, tenantId);
            return CreatedAtAction(nameof(GetUserById), new { tenantId, userId = createdUser.UserId }, createdUser);
        }

        [HttpPut("{tenantId}/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid tenantId, Guid userId, [FromBody] UserDto userDto)
        {
            if (userId != userDto.UserId) return BadRequest();

            var updatedUser = await _globalServices.UpdateUserAsync(userId, userDto);
            return updatedUser != null ? NoContent() : NotFound();
        }

        [HttpDelete("{tenantId}/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid tenantId, Guid userId)
        {
            var deleted = await _globalServices.DeleteUserAsync(userId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
