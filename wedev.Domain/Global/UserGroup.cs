namespace wedev.Domain.Global;

public class UserGroup
{
    public Guid UserId { get; set; } // Part of the composite key from UserTenant
    public Guid TenantId { get; set; } // Part of the composite key from UserTenant
    public UserTenant UserTenant { get; set; } = null!;

    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}