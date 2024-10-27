using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global;

public class App
{
    public Guid AppId { get; set; } = Guid.NewGuid();
    [MaxLength(255)]
    public string Name { get; set; } = null!; // Mandant Name (z.B. "HR")
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}