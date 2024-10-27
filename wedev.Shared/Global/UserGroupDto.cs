namespace wedev.Shared.Global;

public class UserGroupDto
{
    public Guid UserTenantId { get; set; }
    public Guid GroupId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}