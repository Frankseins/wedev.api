using System;
using System.Threading.Tasks;
using wedev.Auth.Shared;
using wedev.Domain.Global;

namespace wedev.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> AuthenticateUserAsync(string userName, string password);
        Task<bool> ValidateTwoFactorCodeAsync(Guid userId, string twoFactorCode);  // Die richtige Signatur für die UserId
        Task<AuthResultDto> RefreshTokenAsync(Guid userId, string refreshToken);
        Task<AuthResultDto> GenerateTokensForUserAsync(User user);
        
        Task<string> GenerateTwoFactorCodeAsync(User user);
        Task SendTwoFactorCodeAsync(User user, string code);
    }
}