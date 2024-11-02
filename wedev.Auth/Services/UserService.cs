using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wedev.Auth.Common;
using wedev.Auth.Interfaces;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Shared.Global;

namespace wedev.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly GlobalDbContext _context;
        private readonly PasswordHashing _passwordHashing;

        public UserService(GlobalDbContext context)
        {
            _context = context;
            _passwordHashing = new PasswordHashing();
        }

        // Benutzerinformationen laden, mit Tenant-Informationen
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.UserTenants)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.UserTenants)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        // Laden aller Benutzer eines bestimmten Tenants
        public async Task<List<UserDto>> GetAllUsersByTenantAsync(Guid tenantId)
        {
            var users = await _context.Users
                .Include(u => u.UserTenants)
                .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
                .ToListAsync();

            return users.Select(MapToDto).ToList();
        }

        // Benutzererstellung mit Passwort-Hashing und Salt
        public async Task<UserDto> CreateUserAsync(UserDto userDto, Guid tenantId)
        {
            var user = MapFromDto(userDto);

            // Passwort-Hash und Salt erzeugen
            var salt = _passwordHashing.CreateSalt();
            var hashedPassword = _passwordHashing.HashPassword(userDto.PasswordHash, salt);
            user.PasswordHash = Convert.ToBase64String(hashedPassword);
            user.PasswordSalt = Convert.ToBase64String(salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        // Benutzerinformationen aktualisieren, inkl. Passwort-Hashing bei Änderung
        public async Task<UserDto?> UpdateUserAsync(Guid userId, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.MobileNumber = userDto.MobileNumber;
            user.IsActive = userDto.IsActive;
            user.IsLocked = userDto.IsLocked;
            user.FailedLoginAttempts = userDto.FailedLoginAttempts;
            user.LockoutEnd = userDto.LockoutEnd;

            if (!string.IsNullOrEmpty(userDto.PasswordHash))
            {
                var salt = _passwordHashing.CreateSalt();
                var hashedPassword = _passwordHashing.HashPassword(userDto.PasswordHash, salt);
                user.PasswordHash = Convert.ToBase64String(hashedPassword);
                user.PasswordSalt = Convert.ToBase64String(salt);
                user.PasswordLastChanged = DateTime.UtcNow;
            }

            user.LastLogin = userDto.LastLogin;
            user.IsSSOEnabled = userDto.IsSSOEnabled;
            user.SSOProvider = userDto.SSOProvider;
            user.SSOUserId = userDto.SSOUserId;
            user.IsTwoFactorEnabled = userDto.IsTwoFactorEnabled;
            user.TwoFactorAttempts = userDto.TwoFactorAttempts;
            user.TwoFactorMethod = userDto.TwoFactorMethod;
            user.TwoFactorSecret = userDto.TwoFactorSecret;
            user.TwoFactorLastVerified = userDto.TwoFactorLastVerified;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userDto.UpdatedBy;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        // Benutzer löschen
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Methoden zur Verwaltung fehlgeschlagener Login- und 2FA-Versuche und Sperrlogik
        public async Task IncrementFailedLoginAttemptsAsync(User user)
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
            {
                user.IsLocked = true;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ResetFailedLoginAttemptsAsync(User user)
        {
            user.FailedLoginAttempts = 0;
            user.IsLocked = false;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();
        }

        public async Task LockUserAsync(User user, TimeSpan lockoutDuration)
        {
            user.IsLocked = true;
            user.LockoutEnd = DateTime.UtcNow.Add(lockoutDuration);
            await _context.SaveChangesAsync();
        }

        public async Task IncrementTwoFactorAttemptsAsync(User user)
        {
            user.TwoFactorAttempts++;
            if (user.TwoFactorAttempts >= 3)
            {
                user.IsLocked = true;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ResetTwoFactorAttemptsAsync(User user)
        {
            user.TwoFactorAttempts = 0;
            await _context.SaveChangesAsync();
        }

        // Passwort-Überprüfung
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var salt = Convert.FromBase64String(storedSalt);
            var hash = Convert.FromBase64String(storedHash);
            return _passwordHashing.VerifyHash(password, salt, hash);
        }

        // Mapping-Methoden zwischen User und UserDto
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
                TwoFactorAttempts = user.TwoFactorAttempts,
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
                TwoFactorAttempts = userDto.TwoFactorAttempts,
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
