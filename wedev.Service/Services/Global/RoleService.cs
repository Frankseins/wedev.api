using Microsoft.EntityFrameworkCore;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Shared.Global;

namespace wedev.Service.Services.Global
{
    public class RoleService
    {
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public RoleService(IDbContextFactory<GlobalDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId, Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var role = await context.Roles
                .Include(r => r.GroupRoles)
                .FirstOrDefaultAsync(r => r.RoleId == roleId && r.TenantId == tenantId);

            return role != null ? MapToDto(role) : null;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync(Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var roles = await context.Roles
                .Where(r => r.TenantId == tenantId)
                .ToListAsync();

            return roles.Select(MapToDto).ToList();
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var role = MapFromDto(roleDto);
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return MapToDto(role);
        }

        public async Task<RoleDto?> UpdateRoleAsync(Guid roleId, RoleDto roleDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var role = await context.Roles.FindAsync(roleId);
            if (role == null) return null;

            role.Name = roleDto.Name;
            role.UpdatedAt = DateTime.UtcNow;

            context.Roles.Update(role);
            await context.SaveChangesAsync();
            return MapToDto(role);
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            using var context = _contextFactory.CreateDbContext();
            var role = await context.Roles.FindAsync(roleId);
            if (role == null) return false;

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return true;
        }

        private RoleDto MapToDto(Role role)
        {
            return new RoleDto
            {
                RoleId = role.RoleId,
                Name = role.Name,
                TenantId = role.TenantId
            };
        }

        private Role MapFromDto(RoleDto roleDto)
        {
            return new Role
            {
                RoleId = roleDto.RoleId,
                Name = roleDto.Name,
                TenantId = roleDto.TenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
