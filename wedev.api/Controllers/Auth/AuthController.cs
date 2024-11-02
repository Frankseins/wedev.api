using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using wedev.Auth.Interfaces;
using wedev.Auth.Shared;

namespace wedev.webapi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService,  IUserService userService)
        {
            _authService = authService;
  _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var result = await _authService.AuthenticateUserAsync(loginRequest.UserName, loginRequest.Password);

            if (!result.IsAuthenticated)
            {
                return Unauthorized(result.Message);
            }

            if (result.RequiresTwoFactor)
            {
                var user = await _userService.GetUserByIdAsync(result.UserId.Value);
                
                // Generiere und sende den 2FA-Code asynchron
                var twoFactorCode = await _authService.GenerateTwoFactorCodeAsync(user);
                await _authService.SendTwoFactorCodeAsync(user, twoFactorCode);

                return Ok(new { requiresTwoFactor = true, userId = result.UserId });
            }

            return Ok(new { token = result.Token, refreshToken = result.RefreshToken });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequestDto verify2FARequest)
        {
            var user = await _userService.GetUserByIdAsync(verify2FARequest.UserId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // Validierung des 2FA-Codes asynchron - nur user.UserId übergeben
            var isValid = await _authService.ValidateTwoFactorCodeAsync(user.UserId, verify2FARequest.TwoFactorCode);

            if (!isValid)
            {
                await _userService.IncrementTwoFactorAttemptsAsync(user);
                return Unauthorized("Invalid or expired two-factor authentication code.");
            }

            // Erfolgreiche 2FA-Validierung -> Zurücksetzen der 2FA-Versuche
            await _userService.ResetTwoFactorAttemptsAsync(user);

            // Token generieren
            var tokenResult = await _authService.GenerateTokensForUserAsync(user);
            return Ok(new { token = tokenResult.Token, refreshToken = tokenResult.RefreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequest.UserId, refreshTokenRequest.RefreshToken);

            if (!result.IsAuthenticated)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { token = result.Token, refreshToken = result.RefreshToken });
        }
    }
}
