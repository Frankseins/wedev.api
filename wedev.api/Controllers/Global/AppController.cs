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
    [Route("global/[controller]")]
    public class AppController : ControllerBase
    {
        private readonly GlobalServices _globalServices;

        public AppController(GlobalServices centralService)
        {
            _globalServices = centralService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppDto>>> GetAllApps()
        {
            return Ok(await _globalServices.GetAllAppsAsync());
        }

        [HttpGet("{appId}")]
        [Authorize]
        public async Task<ActionResult<AppDto>> GetAppById(Guid appId)
        {
            var app = await _globalServices.GetAppByIdAsync(appId);
            return app != null ? Ok(app) : NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateApp([FromBody] AppDto appDto)
        {
            var createdBy = User.Identity.Name ?? "System";
            await _globalServices.CreateAppAsync(appDto, createdBy);
            return CreatedAtAction(nameof(GetAppById), new { appId = appDto.AppId }, appDto);
        }

        [HttpPut("{appId}")]
        [Authorize]
        public async Task<IActionResult> UpdateApp(Guid appId, [FromBody] AppDto appDto)
        {
            if (appId != appDto.AppId) return BadRequest();

            var updatedBy = User.Identity.Name ?? "System";
            await _globalServices.UpdateAppAsync(appDto, updatedBy);
            return NoContent();
        }

        [HttpDelete("{appId}")]
        [Authorize]
        public async Task<IActionResult> DeleteApp(Guid appId)
        {
            await _globalServices.DeleteAppAsync(appId);
            return NoContent();
        }
    }
}
