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
    public class TenantController : ControllerBase
    {
        private readonly GlobalServices _globalServices;

        public TenantController(GlobalServices globalServices)
        {
            _globalServices = globalServices;
        }

        [HttpGet]
       // [Authorize]
        public async Task<ActionResult<IEnumerable<TenantDto>>> GetAllTenants()
        {
            return Ok(await _globalServices.GetAllTenantsAsync());
        }

        [HttpGet("{tenantId}")]
      //  [Authorize]
        public async Task<ActionResult<TenantDto>> GetTenantById(Guid tenantId)
        {
            var tenant = await _globalServices.GetTenantByIdAsync(tenantId);
            return tenant != null ? Ok(tenant) : NotFound();
        }

    [HttpPost]
     //   [Authorize]
        public async Task<IActionResult> CreateTenant([FromBody] TenantDto tenantDto)
        {
            var createdTenant = await _globalServices.CreateTenantAsync(tenantDto);
            return CreatedAtAction(nameof(GetTenantById), new { tenantId = createdTenant.TenantId }, createdTenant);
        }

        [HttpPut("{tenantId}")]
      //  [Authorize]
        public async Task<IActionResult> UpdateTenant(Guid tenantId, [FromBody] TenantDto tenantDto)
        {
            if (tenantId != tenantDto.TenantId) return BadRequest();

            var updatedTenant = await _globalServices.UpdateTenantAsync(tenantId, tenantDto);
            return updatedTenant != null ? NoContent() : NotFound();
        }

        [HttpDelete("{tenantId}")]
       // [Authorize]
        public async Task<IActionResult> DeleteTenant(Guid tenantId)
        {
            var deleted = await _globalServices.DeleteTenantAsync(tenantId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
