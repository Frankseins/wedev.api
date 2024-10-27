namespace wedev.Shared.Global;

public class TenantDto
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public Guid DatabaseId { get; set; }
    public DatabaseDto Database { get; set; } = null!;
    
    // New property to represent the UserTenants relationship
    public List<UserTenantDto> UserTenants { get; set; } = new List<UserTenantDto>();
}