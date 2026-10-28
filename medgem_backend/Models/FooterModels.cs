namespace medgem_backend.Models;

/// <summary>Footer configuration — single-row singleton for global footer settings.</summary>
public class FooterConfig
{
    public int Id { get; set; }
    public string CompanyDescription { get; set; } = "Medgem Global Healthcare assists patients in discovering and coordinating access to quality medical care across leading international destinations.";
    public string CtaLabel { get; set; } = "Free Consultation";
    public string CtaUrl { get; set; } = "/consultation";
    public string CopyrightText { get; set; } = "Medgem Global Healthcare. All rights reserved.";
    public string PrivacyPolicyUrl { get; set; } = "#";
    public string TermsUrl { get; set; } = "#";
    public string CookiePolicyUrl { get; set; } = "#";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>A column in the footer (e.g. Quick Links, Treatments, Destinations, Contact).</summary>
public class FooterColumn
{
    public int Id { get; set; }
    public string Heading { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public bool IsVisible { get; set; } = true;
    public List<FooterLink> Links { get; set; } = [];
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>A single link inside a footer column.</summary>
public class FooterLink
{
    public int Id { get; set; }
    public int FooterColumnId { get; set; }
    public FooterColumn? Column { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsExternal { get; set; } = false;
    public bool IsVisible { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
