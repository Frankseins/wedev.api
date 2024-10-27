using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Shared.Global;

namespace wedev.Service.Services.Global
{
    public class TenantService
    {
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public TenantService(IDbContextFactory<GlobalDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TenantDto?> GetTenantByIdAsync(Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var tenant = await context.Tenants
                .Include(t => t.Database)
                .Include(t => t.UserTenants)
                .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.TenantId == tenantId);

            return tenant != null ? MapToDto(tenant) : null;
        }

        public async Task<List<TenantDto>> GetAllTenantsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var tenants = await context.Tenants
                .Include(t => t.Database)
                .Include(t => t.UserTenants)
                .ThenInclude(ut => ut.User)
                .ToListAsync();

            return tenants.Select(MapToDto).ToList();
        }

        public async Task<TenantDto> CreateTenantAsync(TenantDto tenantDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var tenant = MapFromDto(tenantDto);
            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();
            return MapToDto(tenant);
        }

        public async Task<TenantDto?> UpdateTenantAsync(Guid tenantId, TenantDto tenantDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var tenant = await context.Tenants.FindAsync(tenantId);
            if (tenant == null) return null;

            tenant.Name = tenantDto.Name;
            tenant.DatabaseId = tenantDto.DatabaseId;
            tenant.UpdatedAt = DateTime.UtcNow;

            context.Tenants.Update(tenant);
            await context.SaveChangesAsync();
            return MapToDto(tenant);
        }

        public async Task<bool> DeleteTenantAsync(Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var tenant = await context.Tenants.FindAsync(tenantId);
            if (tenant == null) return false;

            context.Tenants.Remove(tenant);
            await context.SaveChangesAsync();
            return true;
        }

        private TenantDto MapToDto(Tenant tenant)
        {
            return new TenantDto
            {
                TenantId = tenant.TenantId,
                Name = tenant.Name,
                DatabaseId = tenant.DatabaseId,
                Database = new DatabaseDto
                {
                    DatabaseId = tenant.Database.DatabaseId,
                    Name = tenant.Database.Name
                },
                UserTenants = tenant.UserTenants.Select(ut => new UserTenantDto
                {
                    UserId = ut.UserId,
                    TenantId = ut.TenantId
                }).ToList()
            };
        }

        private Tenant MapFromDto(TenantDto tenantDto)
        {
            return new Tenant
            {
                TenantId = tenantDto.TenantId,
                Name = tenantDto.Name,
                DatabaseId = tenantDto.DatabaseId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
