namespace wedev.Auth.JWT
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; } // Lebensdauer des Refresh Tokens
    }
}