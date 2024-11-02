using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wedev.Domain.Global;
using wedev.Shared.Global;

namespace wedev.Auth.Interfaces
{
    public interface IUserService
    {
        // Benutzerinformationen laden
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByNameAsync(string userName);

        // Laden aller Benutzer eines bestimmten Tenants
        Task<List<UserDto>> GetAllUsersByTenantAsync(Guid tenantId);

        // Benutzer erstellen und aktualisieren
        Task<UserDto> CreateUserAsync(UserDto userDto, Guid tenantId);
        Task<UserDto?> UpdateUserAsync(Guid userId, UserDto userDto);

        // Benutzer löschen
        Task<bool> DeleteUserAsync(Guid userId);

        // Sicherheitslogik: Fehlgeschlagene Login- und 2FA-Versuche verwalten
        Task IncrementFailedLoginAttemptsAsync(User user);
        Task ResetFailedLoginAttemptsAsync(User user);
        Task LockUserAsync(User user, TimeSpan lockoutDuration);
        Task IncrementTwoFactorAttemptsAsync(User user);
        Task ResetTwoFactorAttemptsAsync(User user);

        // Passwort-Überprüfung
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }
}