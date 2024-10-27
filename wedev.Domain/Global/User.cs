using System.ComponentModel.DataAnnotations;
namespace wedev.Domain.Global;

public class User
{
 public Guid UserId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string UserName { get; set; } = null!;
    
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? MobileNumber { get; set; } // Optional: Mobile Nummer für SMS-basierte 2FA oder Benachrichtigungen

    // Status Information
    public bool IsActive { get; set; } = true; // Gibt an, ob der Benutzer aktiv ist
    public bool IsLocked { get; set; } = false; // Gibt an, ob das Konto gesperrt ist
    public int FailedLoginAttempts { get; set; } = 0; // Anzahl fehlgeschlagener Login-Versuche
    public DateTime? LockoutEnd { get; set; } // Ende der Sperrfrist nach zu vielen Fehlversuchen

    // Password Information
    [MaxLength(500)]
    public string PasswordHash { get; set; } = null!; // Hash des Passworts
    [MaxLength(500)]
    public string PasswordSalt { get; set; } = null!; // Salt für das Passwort-Hashing
    public DateTime PasswordLastChanged { get; set; } = DateTime.UtcNow; // Letzte Änderung des Passworts
    public DateTime? LastLogin { get; set; } // Zeitpunkt der letzten Anmeldung

    // SSO Information
    public bool IsSSOEnabled { get; set; } = false; // Gibt an, ob SSO aktiviert ist
    public string? SSOProvider { get; set; } // SSO-Anbieter (z.B. "Google", "Microsoft")
    [MaxLength(255)]
    public string? SSOUserId { get; set; } // ID des Benutzers beim SSO-Anbieter

    // 2FA Information
    public bool IsTwoFactorEnabled { get; set; } = false; // Gibt an, ob 2FA aktiviert ist
    public string? TwoFactorMethod { get; set; } // Methode für 2FA (z.B. "Email", "AuthenticatorApp", "SMS")
    
    [MaxLength(500)]
    public string? TwoFactorSecret { get; set; } // Geheimnis für 2FA (z.B. für Authenticator-App)
    public DateTime? TwoFactorLastVerified { get; set; } // Letzte Verifizierung für 2FA

    // Audit Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Zeitstempel der Erstellung
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Zeitstempel der letzten Aktualisierung

    [MaxLength(255)]
    public string CreatedBy { get; set; } = null!; // Wer den Benutzer erstellt hat

    [MaxLength(255)]
    public string UpdatedBy { get; set; } = null!; // Wer den Benutzer zuletzt geändert hat

    // Tenant Relation
    public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}