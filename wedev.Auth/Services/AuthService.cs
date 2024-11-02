using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using wedev.Auth.Interfaces;
using wedev.Auth.Shared;
using wedev.Auth.JWT;
using wedev.Auth.Common;
using wedev.Domain.Global;

namespace wedev.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly PasswordHashing _passwordHashing;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        private static readonly ConcurrentDictionary<Guid, string> RefreshTokens = new ConcurrentDictionary<Guid, string>();
        private static readonly ConcurrentDictionary<Guid, string> TwoFactorCodes = new ConcurrentDictionary<Guid, string>();

        public AuthService(
            IUserService userService, 
            JwtTokenGenerator jwtTokenGenerator,
            PasswordHashing passwordHashing)
        {
            _userService = userService;
            _passwordHashing = passwordHashing;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResultDto> AuthenticateUserAsync(string userName, string password)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            if (user == null || !user.IsActive || user.IsLocked)
            {
                return new AuthResultDto { IsAuthenticated = false, Message = "Account inactive or locked." };
            }

            if (!_passwordHashing.VerifyHash(password, Convert.FromBase64String(user.PasswordSalt), Convert.FromBase64String(user.PasswordHash)))
            {
                await _userService.IncrementFailedLoginAttemptsAsync(user);
                return new AuthResultDto { IsAuthenticated = false, Message = "Invalid credentials." };
            }

            await _userService.ResetFailedLoginAttemptsAsync(user);

            if (user.IsTwoFactorEnabled)
            {
                var twoFactorCode = await GenerateTwoFactorCodeAsync(user);
                await SendTwoFactorCodeAsync(user, twoFactorCode);
                return new AuthResultDto { IsAuthenticated = true, RequiresTwoFactor = true, UserId = user.UserId };
            }

            return await GenerateTokensForUserAsync(user);
        }

        public Task<string> GenerateTwoFactorCodeAsync(User user)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            TwoFactorCodes[user.UserId] = code;  // Speichert den Code temporär im Speicher
            return Task.FromResult(code);
        }

        public Task SendTwoFactorCodeAsync(User user, string code)
        {
            // In einer echten Anwendung würde der Code per E-Mail oder SMS gesendet werden.
            Console.WriteLine($"2FA-Code für Benutzer {user.UserName}: {code}");
            return Task.CompletedTask;
        }

        public async Task<bool> ValidateTwoFactorCodeAsync(Guid userId, string twoFactorCode)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null || !TwoFactorCodes.TryGetValue(userId, out var storedCode) || storedCode != twoFactorCode)
            {
                if (user != null) await _userService.IncrementTwoFactorAttemptsAsync(user);
                return false;
            }

            await _userService.ResetTwoFactorAttemptsAsync(user);
            TwoFactorCodes.TryRemove(userId, out _);  // Entfernt den Code nach erfolgreicher Validierung
            return true;
        }

        public async Task<AuthResultDto> RefreshTokenAsync(Guid userId, string refreshToken)
        {
            if (!RefreshTokens.TryGetValue(userId, out var storedRefreshToken) || storedRefreshToken != refreshToken)
            {
                return new AuthResultDto { IsAuthenticated = false, Message = "Invalid refresh token." };
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new AuthResultDto { IsAuthenticated = false, Message = "User not found." };
            }

            return await GenerateTokensForUserAsync(user);
        }

        public async Task<AuthResultDto> GenerateTokensForUserAsync(User user)
        {
            var token = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            RefreshTokens[user.UserId] = refreshToken;

            return new AuthResultDto { IsAuthenticated = true, Token = token, RefreshToken = refreshToken };
        }
    }
}
