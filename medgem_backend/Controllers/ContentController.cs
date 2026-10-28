using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api")]
public class ContentController(AppDbContext db) : ControllerBase
{
    [HttpGet("homepage")]
    public async Task<ActionResult<ApiResponse<HomePageDto>>> GetHomePage()
    {
        var h = await db.HomePageContent.FirstOrDefaultAsync() ?? new HomePageContent();
        return Ok(new ApiResponse<HomePageDto>(true, "OK", ToHomeDto(h)));
    }

    [Authorize]
    [HttpPut("homepage")]
    public async Task<ActionResult<ApiResponse<HomePageDto>>> UpdateHomePage([FromBody] HomePageDto req)
    {
        var h = await db.HomePageContent.FirstOrDefaultAsync();
        if (h == null) { h = new HomePageContent(); db.HomePageContent.Add(h); }
        h.HeroHeading = req.HeroHeading; h.HeroSubheading = req.HeroSubheading;
        h.HeroBadgeText = req.HeroBadgeText; h.HeroVideoUrl = req.HeroVideoUrl;
        h.HeroPosterUrl = req.HeroPosterUrl; h.HeroCtaPrimaryLabel = req.HeroCtaPrimaryLabel;
        h.HeroCtaPrimaryUrl = req.HeroCtaPrimaryUrl; h.HeroCtaSecondaryLabel = req.HeroCtaSecondaryLabel;
        h.HeroCtaSecondaryUrl = req.HeroCtaSecondaryUrl;
        h.StatPatients = req.StatPatients; h.StatPartners = req.StatPartners;
        h.StatCountries = req.StatCountries; h.StatSpecialties = req.StatSpecialties;
        h.WhyHeading = req.WhyHeading; h.WhySubheading = req.WhySubheading;
        h.SecondOpinionHeading = req.SecondOpinionHeading; h.SecondOpinionSubheading = req.SecondOpinionSubheading;
        h.SecondOpinionImage = req.SecondOpinionImage;
        h.ContactBannerHeading = req.ContactBannerHeading; h.ContactBannerSubheading = req.ContactBannerSubheading;
        h.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<HomePageDto>(true, "Updated", ToHomeDto(h)));
    }

    [HttpGet("settings")]
    public async Task<ActionResult<ApiResponse<SiteSettingsDto>>> GetSettings()
    {
        var s = await db.SiteSettings.FirstOrDefaultAsync() ?? new SiteSettings();
        return Ok(new ApiResponse<SiteSettingsDto>(true, "OK", ToSettingsDto(s)));
    }

    [Authorize]
    [HttpPut("settings")]
    public async Task<ActionResult<ApiResponse<SiteSettingsDto>>> UpdateSettings([FromBody] SiteSettingsDto req)
    {
        var s = await db.SiteSettings.FirstOrDefaultAsync();
        if (s == null) { s = new SiteSettings(); db.SiteSettings.Add(s); }
        s.CompanyName = req.CompanyName; s.LogoUrl = req.LogoUrl; s.Address = req.Address;
        s.ContactEmail = req.ContactEmail; s.ContactPhone = req.ContactPhone;
        s.WhatsAppNumber = req.WhatsAppNumber; s.FacebookUrl = req.FacebookUrl;
        s.TwitterUrl = req.TwitterUrl; s.InstagramUrl = req.InstagramUrl;
        s.LinkedInUrl = req.LinkedInUrl; s.YoutubeUrl = req.YoutubeUrl;
        s.CopyrightText = req.CopyrightText; s.DefaultSeoTitle = req.DefaultSeoTitle;
        s.DefaultSeoDescription = req.DefaultSeoDescription; s.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<SiteSettingsDto>(true, "Updated", ToSettingsDto(s)));
    }

    [Authorize]
    [HttpGet("dashboard")]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetDashboard()
    {
        var recentEnquiries = await db.Enquiries.OrderByDescending(e => e.CreatedAt).Take(5)
            .Select(e => new EnquiryDto(e.Id, e.Name, e.Email, e.PhoneCode, e.Phone,
                e.Treatment, e.Destination, e.Message, e.Type, e.Status, e.InternalNotes,
                e.Country, e.Description, e.Diagnosis, e.CreatedAt, e.UpdatedAt)).ToListAsync();
        var dashboard = new DashboardDto(
            await db.Treatments.CountAsync(),
            await db.Treatments.CountAsync(t => t.Status == "published"),
            await db.Hospitals.CountAsync(),
            await db.Doctors.CountAsync(),
            await db.Destinations.CountAsync(),
            await db.Enquiries.CountAsync(),
            await db.Enquiries.CountAsync(e => e.Status == "new"),
            recentEnquiries);
        return Ok(new ApiResponse<DashboardDto>(true, "OK", dashboard));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("admin/users")]
    public async Task<ActionResult<ApiResponse<List<AdminUserDto>>>> GetAdminUsers()
    {
        var users = await db.AdminUsers.Select(u => new AdminUserDto(u.Id, u.Email, u.FullName,
            u.Role, u.IsActive, u.CreatedAt, u.LastLoginAt)).ToListAsync();
        return Ok(new ApiResponse<List<AdminUserDto>>(true, "OK", users));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("admin/users")]
    public async Task<ActionResult<ApiResponse<AdminUserDto>>> CreateAdminUser([FromBody] CreateAdminRequest req)
    {
        if (await db.AdminUsers.AnyAsync(u => u.Email == req.Email))
            return BadRequest(new ApiResponse<AdminUserDto>(false, "Email already exists", null));
        var user = new AdminUser { Email = req.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password), FullName = req.FullName, Role = req.Role };
        db.AdminUsers.Add(user);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<AdminUserDto>(true, "Created",
            new AdminUserDto(user.Id, user.Email, user.FullName, user.Role, user.IsActive, user.CreatedAt, null)));
    }

    [Authorize]
    [HttpGet("audit-logs")]
    public async Task<ActionResult<ApiResponse<List<AuditLog>>>> GetAuditLogs([FromQuery] int limit = 50)
    {
        var logs = await db.AuditLogs.OrderByDescending(l => l.CreatedAt).Take(limit).ToListAsync();
        return Ok(new ApiResponse<List<AuditLog>>(true, "OK", logs));
    }

    private static HomePageDto ToHomeDto(HomePageContent h) => new(h.Id,
        h.HeroHeading, h.HeroSubheading, h.HeroBadgeText, h.HeroVideoUrl, h.HeroPosterUrl,
        h.HeroCtaPrimaryLabel, h.HeroCtaPrimaryUrl, h.HeroCtaSecondaryLabel, h.HeroCtaSecondaryUrl,
        h.StatPatients, h.StatPartners, h.StatCountries, h.StatSpecialties,
        h.WhyHeading, h.WhySubheading, h.SecondOpinionHeading, h.SecondOpinionSubheading,
        h.SecondOpinionImage, h.ContactBannerHeading, h.ContactBannerSubheading, h.UpdatedAt);

    private static SiteSettingsDto ToSettingsDto(SiteSettings s) => new(s.Id,
        s.CompanyName, s.LogoUrl, s.Address, s.ContactEmail, s.ContactPhone, s.WhatsAppNumber,
        s.FacebookUrl, s.TwitterUrl, s.InstagramUrl, s.LinkedInUrl, s.YoutubeUrl,
        s.CopyrightText, s.DefaultSeoTitle, s.DefaultSeoDescription, s.UpdatedAt);
}
