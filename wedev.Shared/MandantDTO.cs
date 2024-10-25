namespace wedev.Shared;

public class MandantDTO
{
    public Guid MandantId { get; set; }
    public string Name { get; set; }
    public Guid TenantId { get; set; }
    public TenantDTO Tenant { get; set; }
}