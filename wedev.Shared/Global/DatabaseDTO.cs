namespace wedev.Shared.Global;

public class DatabaseDto
{
    public Guid DatabaseId { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid ServerId { get; set; }
    public ServerDto Server { get; set; } = null!;
    public List<TenantDto> Tenants { get; set; } = new List<TenantDto>();
}