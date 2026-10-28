namespace medgem_backend.Models;

public class AuditLog
{
    public int Id { get; set; }
    public int AdminUserId { get; set; }
    public string AdminUserEmail { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
