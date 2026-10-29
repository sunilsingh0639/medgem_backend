namespace medgem_backend.Models;

public class PatientGallery
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string TreatmentName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = "draft"; // draft | published | archived
    public int DisplayOrder { get; set; } = 0;
    public bool ConsentGiven { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<PatientGalleryImage> Images { get; set; } = [];
}

public class PatientGalleryImage
{
    public int Id { get; set; }
    public int PatientGalleryId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public PatientGallery Gallery { get; set; } = null!;
}
