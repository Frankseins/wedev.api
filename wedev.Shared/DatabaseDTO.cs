namespace wedev.Shared;

public class DatabaseDTO
{
    public Guid DatabaseId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid ServerId { get; set; }
    public ServerDTO Server { get; set; }
    public List<TenantDTO> Tenants { get; set; } = new List<TenantDTO>();
}