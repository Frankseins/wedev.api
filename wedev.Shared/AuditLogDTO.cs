namespace wedev.Shared;

public class AuditLogDTO
{
    public Guid AuditLogId { get; set; }
    public string TableName { get; set; }
    public Guid EntityId { get; set; }
    public string ChangeType { get; set; }
    public string ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string Data { get; set; }
}