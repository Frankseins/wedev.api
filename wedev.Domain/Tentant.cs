using System;
using System.Collections.Generic;

namespace wedev.Domain;

public class Tenant
{
    public Guid TenantId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } // Tenant Name (z.B. "Acme Corp")

    public Guid DatabaseId { get; set; } // Foreign Key zur Datenbank
    public Database Database { get; set; } // Navigation Property zur Datenbank

    public ICollection<Mandant> Mandanten { get; set; } = new List<Mandant>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}