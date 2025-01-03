using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Shared.Global
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }

        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!; // Für die Übertragung sensibler Daten vermeiden, falls nicht erforderlich
        [MaxLength(500)]
        public string PasswordSalt { get; set; } = null!; // Für die Übertragung sensibler Daten vermeiden, falls nicht erforderlich
        public DateTime PasswordLastChanged { get; set; }
        public DateTime? LastLogin { get; set; }

        public bool IsSSOEnabled { get; set; }
        public string? SSOProvider { get; set; }

        [MaxLength(255)]
        public string? SSOUserId { get; set; }

        public bool IsTwoFactorEnabled { get; set; }
        public int TwoFactorAttempts { get; set; } // Anzahl der 2FA-Versuche
        public string? TwoFactorMethod { get; set; } // Methode für 2FA (optional)

        [MaxLength(500)]
        public string? TwoFactorSecret { get; set; } // Optional für 2FA-Geheimnis (z.B. für Authenticator)
        public DateTime? TwoFactorLastVerified { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [MaxLength(255)]
        public string CreatedBy { get; set; } = null!;
        [MaxLength(255)]
        public string UpdatedBy { get; set; } = null!;

        // Tenant Information
        public ICollection<UserTenantDto> UserTenants { get; set; } = new List<UserTenantDto>();
    }
}
