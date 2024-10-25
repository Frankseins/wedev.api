using System;

namespace wedev.Domain;
public class Mandant
{
    public Guid MandantId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } // Mandant Name (z.B. "HR")

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}