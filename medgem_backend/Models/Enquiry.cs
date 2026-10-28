namespace medgem_backend.Models;

public class Enquiry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "contact"; // contact | consultation | second-opinion
    public string Status { get; set; } = "new"; // new | in-progress | resolved | closed
    public string InternalNotes { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
