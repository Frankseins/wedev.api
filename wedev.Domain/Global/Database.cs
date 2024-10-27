using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global;
public class Database
{
    public Guid DatabaseId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Name { get; set; } = null!; // Datenbankname

    [MaxLength(100)]
    public string Username { get; set; } = null!; // Optionaler Benutzername

    [MaxLength(100)]
    public string Password { get; set; } = null!; // Optionales Passwort

    public Guid EnvironmentId { get; set; } // Umgebung (Production, Staging, etc.)
    public DeploymentEnvironment Environment { get; set; } = null!;

    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;

    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;


}
