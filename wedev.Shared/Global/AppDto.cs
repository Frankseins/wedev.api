

namespace wedev.Shared.Global;

public class AppDto
{
    public Guid AppId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } // Mandant Name (z.B. "HR")
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    
}

