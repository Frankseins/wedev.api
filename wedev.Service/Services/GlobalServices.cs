using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wedev.Shared.Global;
using wedev.Infrastructure;
using wedev.Service.Services.Global;

namespace wedev.Service.Services;

public class GlobalServices
{
        private readonly AppService _appService;
        private readonly TenantService _tenantService;
        private readonly UserService _userService;
        private readonly GroupService _groupService;
        private readonly RoleService _roleService;
        
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public GlobalServices(
            IDbContextFactory<GlobalDbContext> contextFactory,
            AppService appService,
            TenantService tenantService,
            UserService userService,
            GroupService groupService,
            RoleService roleService)
        {
            _contextFactory = contextFactory;
            _appService = appService;
            _tenantService = tenantService;
            _userService = userService;
            _groupService = groupService;
            _roleService = roleService;
        }

        // AppService methods
        public Task<IEnumerable<AppDto>> GetAllAppsAsync() => _appService.GetAllAppsAsync();
        public Task<AppDto> GetAppByIdAsync(Guid appId) => _appService.GetAppByIdAsync(appId);
        public Task CreateAppAsync(AppDto appDto, string createdBy) => _appService.CreateAppAsync(appDto, createdBy);
        public Task UpdateAppAsync(AppDto appDto, string updatedBy) => _appService.UpdateAppAsync(appDto, updatedBy);
        public Task DeleteAppAsync(Guid appId) => _appService.DeleteAppAsync(appId);

        // TenantService methods
        public Task<TenantDto?> GetTenantByIdAsync(Guid tenantId) => _tenantService.GetTenantByIdAsync(tenantId);
        public Task<List<TenantDto>> GetAllTenantsAsync() => _tenantService.GetAllTenantsAsync();
        public Task<TenantDto> CreateTenantAsync(TenantDto tenantDto) => _tenantService.CreateTenantAsync(tenantDto);
        public Task<TenantDto?> UpdateTenantAsync(Guid tenantId, TenantDto tenantDto) => _tenantService.UpdateTenantAsync(tenantId, tenantDto);
        public Task<bool> DeleteTenantAsync(Guid tenantId) => _tenantService.DeleteTenantAsync(tenantId);

        // UserService methods
        public Task<UserDto?> GetUserByIdAsync(Guid userId, Guid tenantId) => _userService.GetUserByIdAsync(userId, tenantId);
        public Task<List<UserDto>> GetAllUsersByTenantAsync(Guid tenantId) => _userService.GetAllUsersByTenantAsync(tenantId);
        public Task<UserDto> CreateUserAsync(UserDto userDto, Guid tenantId) => _userService.CreateUserAsync(userDto, tenantId);
        public Task<UserDto?> UpdateUserAsync(Guid userId, UserDto userDto) => _userService.UpdateUserAsync(userId, userDto);
        public Task<bool> DeleteUserAsync(Guid userId) => _userService.DeleteUserAsync(userId);

        // GroupService methods
        public Task<GroupDto?> GetGroupByIdAsync(Guid groupId, Guid tenantId) => _groupService.GetGroupByIdAsync(groupId, tenantId);
        public Task<List<GroupDto>> GetAllGroupsAsync(Guid tenantId) => _groupService.GetAllGroupsAsync(tenantId);
        public Task<GroupDto> CreateGroupAsync(GroupDto groupDto) => _groupService.CreateGroupAsync(groupDto);
        public Task<GroupDto?> UpdateGroupAsync(Guid groupId, GroupDto groupDto) => _groupService.UpdateGroupAsync(groupId, groupDto);
        public Task<bool> DeleteGroupAsync(Guid groupId) => _groupService.DeleteGroupAsync(groupId);

        // RoleService methods
        public Task<RoleDto?> GetRoleByIdAsync(Guid roleId, Guid tenantId) => _roleService.GetRoleByIdAsync(roleId, tenantId);
        public Task<List<RoleDto>> GetAllRolesAsync(Guid tenantId) => _roleService.GetAllRolesAsync(tenantId);
        public Task<RoleDto> CreateRoleAsync(RoleDto roleDto) => _roleService.CreateRoleAsync(roleDto);
        public Task<RoleDto?> UpdateRoleAsync(Guid roleId, RoleDto roleDto) => _roleService.UpdateRoleAsync(roleId, roleDto);
        public Task<bool> DeleteRoleAsync(Guid roleId) => _roleService.DeleteRoleAsync(roleId);
}
