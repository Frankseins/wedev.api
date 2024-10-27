namespace wedev.Shared.Global;

public class MandantDto
{
    public Guid MandantId { get; set; }
    public string Name { get; set; } = null!;
    public Guid TenantId { get; set; }
    public TenantDto Tenant { get; set; } = null!;
}