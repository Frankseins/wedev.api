namespace wedev.Shared.Global;

public class GroupDto
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
    public Guid TenantId { get; set; }
    public List<UserGroupDto> UserGroups { get; set; } = new List<UserGroupDto>();
    public List<GroupRoleDto> GroupRoles { get; set; } = new List<GroupRoleDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}