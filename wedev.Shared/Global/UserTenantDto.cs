namespace wedev.Shared.Global;

public class UserTenantDto
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public List<UserGroupDto> UserGroups { get; set; } = new List<UserGroupDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}