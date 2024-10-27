namespace wedev.Domain.Global;

public class AuditLog
{
    public Guid AuditLogId { get; set; } = Guid.NewGuid();
    public string TableName { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string ChangeType { get; set; } = null!; // INSERT, UPDATE, DELETE
    public string ChangedBy { get; set; } = null!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string Data { get; set; } = null!; // Geänderte Daten im JSON-Format
}