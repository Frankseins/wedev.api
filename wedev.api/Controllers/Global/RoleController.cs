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
    public class RoleController : ControllerBase
    {
        private readonly GlobalServices _globalServices;

        public RoleController(GlobalServices globalServices)
        {
            _globalServices = globalServices;
        }

        [HttpGet("{tenantId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles(Guid tenantId)
        {
            return Ok(await _globalServices.GetAllRolesAsync(tenantId));
        }

        [HttpGet("{tenantId}/{roleId}")]
        [Authorize]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid tenantId, Guid roleId)
        {
            var role = await _globalServices.GetRoleByIdAsync(roleId, tenantId);
            return role != null ? Ok(role) : NotFound();
        }

        [HttpPost("{tenantId}")]
        [Authorize]
        public async Task<IActionResult> CreateRole(Guid tenantId, [FromBody] RoleDto roleDto)
        {
            roleDto.TenantId = tenantId; // Ensures tenant assignment
            var createdRole = await _globalServices.CreateRoleAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { tenantId, roleId = createdRole.RoleId }, createdRole);
        }

        [HttpPut("{tenantId}/{roleId}")]
        [Authorize]
        public async Task<IActionResult> UpdateRole(Guid tenantId, Guid roleId, [FromBody] RoleDto roleDto)
        {
            if (roleId != roleDto.RoleId) return BadRequest();

            var updatedRole = await _globalServices.UpdateRoleAsync(roleId, roleDto);
            return updatedRole != null ? NoContent() : NotFound();
        }

        [HttpDelete("{tenantId}/{roleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRole(Guid tenantId, Guid roleId)
        {
            var deleted = await _globalServices.DeleteRoleAsync(roleId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
