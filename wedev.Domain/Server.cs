using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain;
public class Server
{
    public Guid ServerId { get; set; } = Guid.NewGuid();
    
    [MaxLength(255)]
    public string Name { get; set; } // Servername (z.B. "Server1")

    [MaxLength(255)]
    public string Host { get; set; } // Hostname oder IP-Adresse des Servers

    public int Port { get; set; } = 5432; // Standardport für PostgreSQL

    [MaxLength(500)]
    public string AdditionalParams { get; set; } // Zusätzliche Verbindungsparameter

    [MaxLength(100)]
    public string Username { get; set; } // Standard-Benutzername

    [MaxLength(100)]
    public string Password { get; set; } // Standard-Passwort

    public Guid EnvironmentId { get; set; } // Umgebung (Production, Staging, etc.)
    public DeploymentEnvironment Environment { get; set; } // Navigation Property zur Umgebung

    public ICollection<Database> Databases { get; set; } = new List<Database>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    public string BuildConnectionString()
    {
        var connectionString = $"Host={Host};Port={Port};{AdditionalParams}";
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            connectionString += $";Username={Username};Password={Password}";
        }
        return connectionString;
    }
}