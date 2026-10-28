namespace medgem_backend.DTOs;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token, string Email, string FullName, string Role);

public record TreatmentDto(int Id, string Title, string Slug, string Category, string Description,
    string Image, string Icon, string Duration, string Destinations, string Tags,
    string Status, int DisplayOrder, string SeoTitle, string SeoDescription,
    DateTime CreatedAt, DateTime UpdatedAt);

public record TreatmentUpsertRequest(string Title, string Slug, string Category, string Description,
    string Image, string Icon, string Duration, string Destinations, string Tags,
    string Status, int DisplayOrder, string SeoTitle, string SeoDescription);

public record HospitalDto(int Id, string Name, string Slug, string City, string Country,
    string Image, string Specialties, string Accreditation, string Description,
    int Beds, int Established, decimal Rating, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription, DateTime CreatedAt, DateTime UpdatedAt);

public record HospitalUpsertRequest(string Name, string Slug, string City, string Country,
    string Image, string Specialties, string Accreditation, string Description,
    int Beds, int Established, decimal Rating, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription);

public record DestinationDto(int Id, string Country, string City, string Slug, string Tagline,
    string Description, string Image, string Treatments, int Hospitals, int Specialists,
    string Flag, string Highlights, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription, DateTime CreatedAt, DateTime UpdatedAt);

public record DestinationUpsertRequest(string Country, string City, string Slug, string Tagline,
    string Description, string Image, string Treatments, int Hospitals, int Specialists,
    string Flag, string Highlights, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription);

public record DoctorDto(int Id, string Name, string Slug, string Specialty, string Hospital,
    string Country, string Experience, string Languages, string Image, string Education,
    string About, string Tags, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription, DateTime CreatedAt, DateTime UpdatedAt);

public record DoctorUpsertRequest(string Name, string Slug, string Specialty, string Hospital,
    string Country, string Experience, string Languages, string Image, string Education,
    string About, string Tags, string Status, int DisplayOrder,
    string SeoTitle, string SeoDescription);

public record TestimonialDto(int Id, string Name, string Country, string Treatment,
    string Avatar, string Text, int Rating, string Note, string Status, int DisplayOrder,
    DateTime CreatedAt, DateTime UpdatedAt);

public record TestimonialUpsertRequest(string Name, string Country, string Treatment,
    string Avatar, string Text, int Rating, string Note, string Status, int DisplayOrder);

public record FaqDto(int Id, string Question, string Answer, string Category,
    string Status, int DisplayOrder, DateTime CreatedAt, DateTime UpdatedAt);

public record FaqUpsertRequest(string Question, string Answer, string Category,
    string Status, int DisplayOrder);

public record EnquiryDto(int Id, string Name, string Email, string PhoneCode, string Phone,
    string Treatment, string Destination, string Message, string Type, string Status,
    string InternalNotes, string Country, string Description, string Diagnosis,
    DateTime CreatedAt, DateTime UpdatedAt);

public record ContactSubmitRequest(string Name, string Email, string PhoneCode, string Phone,
    string Treatment, string Destination, string Message);

public record ConsultationSubmitRequest(string Name, string Email, string Phone, string Country,
    string Treatment, string Destination, string Description);

public record SecondOpinionSubmitRequest(string Name, string Email, string Phone,
    string Treatment, string Diagnosis, string Description);

public record EnquiryUpdateRequest(string Status, string InternalNotes);

public record HomePageDto(int Id, string HeroHeading, string HeroSubheading, string HeroBadgeText,
    string HeroVideoUrl, string HeroPosterUrl, string HeroCtaPrimaryLabel, string HeroCtaPrimaryUrl,
    string HeroCtaSecondaryLabel, string HeroCtaSecondaryUrl,
    int StatPatients, int StatPartners, int StatCountries, int StatSpecialties,
    string WhyHeading, string WhySubheading,
    string SecondOpinionHeading, string SecondOpinionSubheading, string SecondOpinionImage,
    string ContactBannerHeading, string ContactBannerSubheading, DateTime UpdatedAt);

public record SiteSettingsDto(int Id, string CompanyName, string LogoUrl, string Address,
    string ContactEmail, string ContactPhone, string WhatsAppNumber,
    string FacebookUrl, string TwitterUrl, string InstagramUrl, string LinkedInUrl, string YoutubeUrl,
    string CopyrightText, string DefaultSeoTitle, string DefaultSeoDescription, DateTime UpdatedAt);

public record AdminUserDto(int Id, string Email, string FullName, string Role, bool IsActive,
    DateTime CreatedAt, DateTime? LastLoginAt);

public record CreateAdminRequest(string Email, string Password, string FullName, string Role);

public record DashboardDto(int TotalTreatments, int PublishedTreatments,
    int TotalHospitals, int TotalDoctors, int TotalDestinations,
    int TotalEnquiries, int NewEnquiries, List<EnquiryDto> RecentEnquiries);

public record PagedResult<T>(List<T> Items, int Total, int Page, int PageSize);

public record MediaAssetDto(int Id, string FileName, string OriginalName, string Url,
    string AltText, string Caption, string MimeType, long FileSize, DateTime CreatedAt);

public record ApiResponse<T>(bool Success, string Message, T? Data);
