namespace medgem_backend.Models;

public class Destination
{
    public int Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Treatments { get; set; } = string.Empty; // JSON array
    public int Hospitals { get; set; }
    public int Specialists { get; set; }
    public string Flag { get; set; } = string.Empty;
    public string Highlights { get; set; } = string.Empty; // JSON array
    public string Status { get; set; } = "published";
    public int DisplayOrder { get; set; } = 0;
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
