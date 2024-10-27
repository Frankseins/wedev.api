using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global;

public class Group
{
    public Guid GroupId { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string Name { get; set; } = null!;

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    public ICollection<GroupRole> GroupRoles { get; set; } = new List<GroupRole>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}