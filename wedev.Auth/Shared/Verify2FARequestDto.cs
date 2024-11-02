using System;

namespace wedev.Auth.Shared
{
    public class Verify2FARequestDto
    {
        public Guid UserId { get; set; }
        public string TwoFactorCode { get; set; } = string.Empty;
    }
}