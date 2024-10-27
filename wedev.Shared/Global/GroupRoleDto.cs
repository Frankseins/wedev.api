namespace wedev.Shared.Global;

public class GroupRoleDto
{
    public Guid GroupId { get; set; }
    public Guid RoleId { get; set; }
    public Guid TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}