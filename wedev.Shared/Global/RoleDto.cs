namespace wedev.Shared.Global;

public class RoleDto
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public Guid TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}