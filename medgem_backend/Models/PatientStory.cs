namespace medgem_backend.Models;

public class PatientStory
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;      // upload path or external URL
    public string VideoTitle { get; set; } = string.Empty;
    public string VideoProvider { get; set; } = string.Empty; // upload | youtube | vimeo
    public bool IsFeatured { get; set; } = false;
    public string Status { get; set; } = "draft"; // draft | published | archived
    public int DisplayOrder { get; set; } = 0;
    public bool ConsentGiven { get; set; } = false;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<PatientStoryPhoto> Photos { get; set; } = [];
}

public class PatientStoryPhoto
{
    public int Id { get; set; }
    public int PatientStoryId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public PatientStory Story { get; set; } = null!;
}
