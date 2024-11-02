using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string UserName { get; set; } = null!;
        
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        // Status Information
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        // Password Information
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;
        [MaxLength(500)]
        public string PasswordSalt { get; set; } = null!;
        public DateTime PasswordLastChanged { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // 2FA Information
        public bool IsTwoFactorEnabled { get; set; } = false;
        public int TwoFactorAttempts { get; set; } = 0;

        [MaxLength(50)]
        public string? TwoFactorMethod { get; set; } // Neu hinzugefügt: Methode für 2FA (z.B. "Email", "AuthenticatorApp", "SMS")

        [MaxLength(500)]
        public string? TwoFactorSecret { get; set; }
        public DateTime? TwoFactorLastVerified { get; set; }

        // SSO Information
        public bool IsSSOEnabled { get; set; } = false;
        public string? SSOProvider { get; set; }
        [MaxLength(255)]
        public string? SSOUserId { get; set; }

        // Audit Information
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string CreatedBy { get; set; } = null!;
        [MaxLength(255)]
        public string UpdatedBy { get; set; } = null!;

        // Tenant Relation
        public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
    }
}
