namespace wedev.Shared;

public class TenantDTO
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public Guid DatabaseId { get; set; }
    public DatabaseDTO Database { get; set; }
    public List<MandantDTO> Mandanten { get; set; } = new List<MandantDTO>();
}