namespace wedev.Auth.Shared
{
    public class AuthResultDto
    {
        public bool IsAuthenticated { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
        public Guid? UserId { get; set; }
    }
}