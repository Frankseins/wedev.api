using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using wedev.Domain.Global;

namespace wedev.Domain.Global;

public class Tenant
{
    public Guid TenantId { get; set; } = Guid.NewGuid();
    [MaxLength(255)]
    public string Name { get; set; } = null!; // Tenant Name (z.B. "Acme Corp")

    public Guid DatabaseId { get; set; } // Foreign Key zur Datenbank
    public Database Database { get; set; } = null!; // Navigation Property zur Datenbank

    public ICollection<App> Apps { get; set; } = new List<App>();
    public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}