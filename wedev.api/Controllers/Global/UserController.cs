using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wedev.Auth.Interfaces;
using wedev.Shared.Global;

namespace wedev.webapi.Controllers.Global;

    [ApiController]
    [Route("Global/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/User/username/{username}
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByName(string username)
        {
            var user = await _userService.GetUserByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/User/tenant/{tenantId}
        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersByTenant(Guid tenantId)
        {
            var users = await _userService.GetAllUsersByTenantAsync(tenantId);
            return Ok(users);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto, [FromQuery] Guid tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateUserAsync(userDto, tenantId);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/User/{id}/increment-failed-login
        [HttpPost("{id}/increment-failed-login")]
        public async Task<IActionResult> IncrementFailedLoginAttempts(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.IncrementFailedLoginAttemptsAsync(user);
            return NoContent();
        }

        // POST: api/User/{id}/reset-failed-login
        [HttpPost("{id}/reset-failed-login")]
        public async Task<IActionResult> ResetFailedLoginAttempts(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.ResetFailedLoginAttemptsAsync(user);
            return NoContent();
        }

        // POST: api/User/{id}/lock
        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockUser(Guid id, [FromQuery] int minutes)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.LockUserAsync(user, TimeSpan.FromMinutes(minutes));
            return NoContent();
        }

        // POST: api/User/{id}/increment-2fa-attempts
        [HttpPost("{id}/increment-2fa-attempts")]
        public async Task<IActionResult> IncrementTwoFactorAttempts(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.IncrementTwoFactorAttemptsAsync(user);
            return NoContent();
        }

        // POST: api/User/{id}/reset-2fa-attempts
        [HttpPost("{id}/reset-2fa-attempts")]
        public async Task<IActionResult> ResetTwoFactorAttempts(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.ResetTwoFactorAttemptsAsync(user);
            return NoContent();
        }

   
    }

