namespace wedev.Domain;

public class AuditLog
{
    public Guid AuditLogId { get; set; } = Guid.NewGuid();
    public string TableName { get; set; }
    public Guid EntityId { get; set; }
    public string ChangeType { get; set; } // INSERT, UPDATE, DELETE
    public string ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string Data { get; set; } // Geänderte Daten im JSON-Format
}