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
    public class GroupController : ControllerBase
    {
        private readonly GlobalServices _globalServices;

        public GroupController(GlobalServices globalServices)
        {
            _globalServices = globalServices;
        }

        [HttpGet("{tenantId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAllGroups(Guid tenantId)
        {
            return Ok(await _globalServices.GetAllGroupsAsync(tenantId));
        }

        [HttpGet("{tenantId}/{groupId}")]
        [Authorize]
        public async Task<ActionResult<GroupDto>> GetGroupById(Guid tenantId, Guid groupId)
        {
            var group = await _globalServices.GetGroupByIdAsync(groupId, tenantId);
            return group != null ? Ok(group) : NotFound();
        }

        [HttpPost("{tenantId}")]
        [Authorize]
        public async Task<IActionResult> CreateGroup(Guid tenantId, [FromBody] GroupDto groupDto)
        {
            groupDto.TenantId = tenantId;  // Ensures tenant assignment
            var createdGroup = await _globalServices.CreateGroupAsync(groupDto);
            return CreatedAtAction(nameof(GetGroupById), new { tenantId, groupId = createdGroup.GroupId }, createdGroup);
        }

        [HttpPut("{tenantId}/{groupId}")]
        [Authorize]
        public async Task<IActionResult> UpdateGroup(Guid tenantId, Guid groupId, [FromBody] GroupDto groupDto)
        {
            if (groupId != groupDto.GroupId) return BadRequest();

            var updatedGroup = await _globalServices.UpdateGroupAsync(groupId, groupDto);
            return updatedGroup != null ? NoContent() : NotFound();
        }

        [HttpDelete("{tenantId}/{groupId}")]
        [Authorize]
        public async Task<IActionResult> DeleteGroup(Guid tenantId, Guid groupId)
        {
            var deleted = await _globalServices.DeleteGroupAsync(groupId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
