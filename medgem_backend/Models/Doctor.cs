namespace medgem_backend.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty; // JSON array
    public string Image { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty; // JSON array
    public string Status { get; set; } = "published";
    public int DisplayOrder { get; set; } = 0;
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
