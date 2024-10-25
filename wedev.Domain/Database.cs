using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain;
public class Database
{
    public Guid DatabaseId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Name { get; set; } // Datenbankname

    [MaxLength(100)]
    public string Username { get; set; } // Optionaler Benutzername

    [MaxLength(100)]
    public string Password { get; set; } // Optionales Passwort

    public Guid EnvironmentId { get; set; } // Umgebung (Production, Staging, etc.)
    public DeploymentEnvironment Environment { get; set; }

    public Guid ServerId { get; set; }
    public Server Server { get; set; }

    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    public string BuildConnectionString()
    {
        var connectionString = $"{Server.BuildConnectionString()};Database={Name}";
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            connectionString += $";Username={Username};Password={Password}";
        }
        return connectionString;
    }
}
