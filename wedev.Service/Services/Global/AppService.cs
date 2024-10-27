using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wedev.Shared.Global;
using wedev.Domain.Global;
using wedev.Infrastructure;
namespace wedev.Service.Services.Global
{
    public class AppService
    {
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public AppService(IDbContextFactory<GlobalDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Mapping methods
        private AppDto MapToDto(App app)
        {
            return new AppDto
            {
                AppId = app.AppId,
                Name = app.Name,
                CreatedAt = app.CreatedAt,
                UpdatedAt = app.UpdatedAt,
                CreatedBy = app.CreatedBy,
                UpdatedBy = app.UpdatedBy
            };
        }

        private App MapFromDto(AppDto dto)
        {
            return new App
            {
                AppId = dto.AppId,
                Name = dto.Name,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                CreatedBy = dto.CreatedBy,
                UpdatedBy = dto.UpdatedBy
            };
        }

        // Service methods
        public async Task<IEnumerable<AppDto>> GetAllAppsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var apps = await context.Apps.ToListAsync();
            return apps.Select(MapToDto);
        }

        public async Task<AppDto> GetAppByIdAsync(Guid appId)
        {
            using var context = _contextFactory.CreateDbContext();
            var app = await context.Apps.FindAsync(appId);
            return app != null ? MapToDto(app) : null;
        }

        public async Task<AppDto> CreateAppAsync(AppDto appDto, string createdBy)
        {
            using var context = _contextFactory.CreateDbContext();
            var app = MapFromDto(appDto);
            app.CreatedBy = createdBy;
            app.UpdatedBy = createdBy;
            app.CreatedAt = DateTime.UtcNow;
            app.UpdatedAt = DateTime.UtcNow;

            context.Apps.Add(app);
            await context.SaveChangesAsync();

            return MapToDto(app);
        }

        public async Task<AppDto> UpdateAppAsync(AppDto appDto, string updatedBy)
        {
            using var context = _contextFactory.CreateDbContext();
            var app = await context.Apps.FindAsync(appDto.AppId);
            if (app != null)
            {
                app.Name = appDto.Name;
                app.UpdatedBy = updatedBy;
                app.UpdatedAt = DateTime.UtcNow;

                await context.SaveChangesAsync();
                return MapToDto(app);
            }
            return null;
        }

        public async Task<bool> DeleteAppAsync(Guid appId)
        {
            using var context = _contextFactory.CreateDbContext();
            var app = await context.Apps.FindAsync(appId);
            if (app != null)
            {
                context.Apps.Remove(app);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
