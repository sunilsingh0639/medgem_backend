namespace medgem_backend.Models;

public class SiteSettings
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = "Medgem Global Healthcare";
    public string LogoUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string FacebookUrl { get; set; } = string.Empty;
    public string TwitterUrl { get; set; } = string.Empty;
    public string InstagramUrl { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string YoutubeUrl { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public string DefaultSeoTitle { get; set; } = string.Empty;
    public string DefaultSeoDescription { get; set; } = string.Empty;
    //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
