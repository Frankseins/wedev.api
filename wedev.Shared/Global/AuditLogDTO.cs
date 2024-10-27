namespace wedev.Shared.Global;

public class AuditLogDto
{
    public Guid AuditLogId { get; set; }
    public string TableName { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string ChangeType { get; set; } = null!;
    public string ChangedBy { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
    public string Data { get; set; } = null!;
}