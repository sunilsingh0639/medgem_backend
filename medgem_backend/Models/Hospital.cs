namespace medgem_backend.Models;

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Specialties { get; set; } = string.Empty; // JSON array
    public string Accreditation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Beds { get; set; }
    public int Established { get; set; }
    public decimal Rating { get; set; }
    public string Status { get; set; } = "published";
    public int DisplayOrder { get; set; } = 0;
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
