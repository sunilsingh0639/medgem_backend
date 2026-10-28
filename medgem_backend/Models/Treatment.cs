namespace medgem_backend.Models;

public class Treatment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Destinations { get; set; } = string.Empty; // JSON array
    public string Tags { get; set; } = string.Empty; // JSON array
    public string Status { get; set; } = "published"; // published | draft | archived
    public int DisplayOrder { get; set; } = 0;
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
