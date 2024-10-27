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
    public class UserService
    {
        private readonly IDbContextFactory<GlobalDbContext> _contextFactory;

        public UserService(IDbContextFactory<GlobalDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId, Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.Users
                .Include(u => u.UserTenants)
                .ThenInclude(ut => ut.Tenant)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.UserTenants.Any(ut => ut.TenantId == tenantId));

            return user != null ? MapToDto(user) : null;
        }

        public async Task<List<UserDto>> GetAllUsersByTenantAsync(Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var users = await context.Users
                .Include(u => u.UserTenants)
                .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
                .ToListAsync();

            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto, Guid tenantId)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = MapFromDto(userDto);
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid userId, UserDto userDto)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            // Update properties
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.MobileNumber = userDto.MobileNumber;
            user.IsActive = userDto.IsActive;
            user.IsLocked = userDto.IsLocked;
            user.FailedLoginAttempts = userDto.FailedLoginAttempts;
            user.LockoutEnd = userDto.LockoutEnd;
            user.PasswordHash = userDto.PasswordHash;
            user.PasswordSalt = userDto.PasswordSalt;
            user.PasswordLastChanged = userDto.PasswordLastChanged;
            user.LastLogin = userDto.LastLogin;
            user.IsSSOEnabled = userDto.IsSSOEnabled;
            user.SSOProvider = userDto.SSOProvider;
            user.SSOUserId = userDto.SSOUserId;
            user.IsTwoFactorEnabled = userDto.IsTwoFactorEnabled;
            user.TwoFactorMethod = userDto.TwoFactorMethod;
            user.TwoFactorSecret = userDto.TwoFactorSecret;
            user.TwoFactorLastVerified = userDto.TwoFactorLastVerified;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userDto.UpdatedBy;

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                IsActive = user.IsActive,
                IsLocked = user.IsLocked,
                FailedLoginAttempts = user.FailedLoginAttempts,
                LockoutEnd = user.LockoutEnd,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                PasswordLastChanged = user.PasswordLastChanged,
                LastLogin = user.LastLogin,
                IsSSOEnabled = user.IsSSOEnabled,
                SSOProvider = user.SSOProvider,
                SSOUserId = user.SSOUserId,
                IsTwoFactorEnabled = user.IsTwoFactorEnabled,
                TwoFactorMethod = user.TwoFactorMethod,
                TwoFactorSecret = user.TwoFactorSecret,
                TwoFactorLastVerified = user.TwoFactorLastVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy
            };
        }

        private User MapFromDto(UserDto userDto)
        {
            return new User
            {
                UserId = userDto.UserId,
                UserName = userDto.UserName,
                Email = userDto.Email,
                MobileNumber = userDto.MobileNumber,
                IsActive = userDto.IsActive,
                IsLocked = userDto.IsLocked,
                FailedLoginAttempts = userDto.FailedLoginAttempts,
                LockoutEnd = userDto.LockoutEnd,
                PasswordHash = userDto.PasswordHash,
                PasswordSalt = userDto.PasswordSalt,
                PasswordLastChanged = userDto.PasswordLastChanged,
                LastLogin = userDto.LastLogin,
                IsSSOEnabled = userDto.IsSSOEnabled,
                SSOProvider = userDto.SSOProvider,
                SSOUserId = userDto.SSOUserId,
                IsTwoFactorEnabled = userDto.IsTwoFactorEnabled,
                TwoFactorMethod = userDto.TwoFactorMethod,
                TwoFactorSecret = userDto.TwoFactorSecret,
                TwoFactorLastVerified = userDto.TwoFactorLastVerified,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userDto.CreatedBy,
                UpdatedBy = userDto.UpdatedBy
            };
        }
    }
}
