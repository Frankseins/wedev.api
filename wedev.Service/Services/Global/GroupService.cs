using Microsoft.EntityFrameworkCore;
using wedev.Infrastructure;
using wedev.Shared.Global;
using wedev.Domain.Global;

namespace wedev.Service.Services.Global
{
    public class GroupService
    {
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public GroupService(IDbContextFactory<GlobalDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GroupDto?> GetGroupByIdAsync(Guid groupId, Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var group = await context.Groups
                .Include(g => g.GroupRoles)
                .Include(g => g.UserGroups)
                .FirstOrDefaultAsync(g => g.GroupId == groupId && g.TenantId == tenantId);

            return group != null ? MapToDto(group) : null;
        }

        public async Task<List<GroupDto>> GetAllGroupsAsync(Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var groups = await context.Groups
                .Where(g => g.TenantId == tenantId)
                .ToListAsync();

            return groups.Select(MapToDto).ToList();
        }

        public async Task<GroupDto> CreateGroupAsync(GroupDto groupDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var group = MapFromDto(groupDto);
            context.Groups.Add(group);
            await context.SaveChangesAsync();
            return MapToDto(group);
        }

        public async Task<GroupDto?> UpdateGroupAsync(Guid groupId, GroupDto groupDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var group = await context.Groups.FindAsync(groupId);
            if (group == null) return null;

            group.Name = groupDto.Name;
            group.UpdatedAt = DateTime.UtcNow;

            context.Groups.Update(group);
            await context.SaveChangesAsync();
            return MapToDto(group);
        }

        public async Task<bool> DeleteGroupAsync(Guid groupId)
        {
            using var context = _contextFactory.CreateDbContext();
            var group = await context.Groups.FindAsync(groupId);
            if (group == null) return false;

            context.Groups.Remove(group);
            await context.SaveChangesAsync();
            return true;
        }

        private GroupDto MapToDto(Group group)
        {
            return new GroupDto
            {
                GroupId = group.GroupId,
                Name = group.Name,
                TenantId = group.TenantId
            };
        }

        private Group MapFromDto(GroupDto groupDto)
        {
            return new Group
            {
                GroupId = groupDto.GroupId,
                Name = groupDto.Name,
                TenantId = groupDto.TenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
