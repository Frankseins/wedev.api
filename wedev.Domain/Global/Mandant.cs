using System;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global;
public class Mandant
{
    public Guid MandantId { get; set; } = Guid.NewGuid();
    [MaxLength(255)]
    public string Name { get; set; } = null!; // Mandant Name (z.B. "HR")

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}