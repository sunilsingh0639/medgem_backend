using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

/// <summary>Manages footer configuration, columns, and links.</summary>
[ApiController]
[Route("api/footer")]
public class FooterController(AppDbContext db) : ControllerBase
{
    /// <summary>Get complete public footer data (config + columns + site settings).</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PublicFooterDto>>> GetPublic()
    {
        var config = await db.FooterConfigs.FirstOrDefaultAsync() ?? new FooterConfig();
        var columns = await db.FooterColumns
            .Where(c => c.IsVisible)
            .OrderBy(c => c.DisplayOrder)
            .Include(c => c.Links.Where(l => l.IsVisible).OrderBy(l => l.DisplayOrder))
            .ToListAsync();
        var settings = await db.SiteSettings.FirstOrDefaultAsync() ?? new SiteSettings();

        return Ok(new ApiResponse<PublicFooterDto>(true, "OK", new PublicFooterDto(
            ToConfigDto(config),
            columns.Select(ToColumnDto).ToList(),
            ToSettingsDto(settings))));
    }

    // ── Footer Config ────────────────────────────────────────────────────────

    /// <summary>Get footer config (admin).</summary>
    [Authorize]
    [HttpGet("config")]
    public async Task<ActionResult<ApiResponse<FooterConfigDto>>> GetConfig()
    {
        var c = await db.FooterConfigs.FirstOrDefaultAsync() ?? new FooterConfig();
        return Ok(new ApiResponse<FooterConfigDto>(true, "OK", ToConfigDto(c)));
    }

    /// <summary>Update footer config.</summary>
    [Authorize]
    [HttpPut("config")]
    public async Task<ActionResult<ApiResponse<FooterConfigDto>>> UpdateConfig([FromBody] FooterConfigDto req)
    {
        var c = await db.FooterConfigs.FirstOrDefaultAsync();
        if (c == null) { c = new FooterConfig(); db.FooterConfigs.Add(c); }
        c.CompanyDescription = req.CompanyDescription; c.CtaLabel = req.CtaLabel;
        c.CtaUrl = req.CtaUrl; c.CopyrightText = req.CopyrightText;
        c.PrivacyPolicyUrl = req.PrivacyPolicyUrl; c.TermsUrl = req.TermsUrl;
        c.CookiePolicyUrl = req.CookiePolicyUrl; c.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FooterConfigDto>(true, "Updated", ToConfigDto(c)));
    }

    // ── Footer Columns ───────────────────────────────────────────────────────

    /// <summary>Get all footer columns with links (admin).</summary>
    [Authorize]
    [HttpGet("columns")]
    public async Task<ActionResult<ApiResponse<List<FooterColumnDto>>>> GetColumns()
    {
        var cols = await db.FooterColumns.OrderBy(c => c.DisplayOrder)
            .Include(c => c.Links.OrderBy(l => l.DisplayOrder)).ToListAsync();
        return Ok(new ApiResponse<List<FooterColumnDto>>(true, "OK", cols.Select(ToColumnDto).ToList()));
    }

    /// <summary>Create a footer column.</summary>
    [Authorize]
    [HttpPost("columns")]
    public async Task<ActionResult<ApiResponse<FooterColumnDto>>> CreateColumn([FromBody] FooterColumnUpsertRequest req)
    {
        var col = new FooterColumn { Heading = req.Heading, DisplayOrder = req.DisplayOrder, IsVisible = req.IsVisible };
        db.FooterColumns.Add(col);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FooterColumnDto>(true, "Created", ToColumnDto(col)));
    }

    /// <summary>Update a footer column.</summary>
    [Authorize]
    [HttpPut("columns/{id}")]
    public async Task<ActionResult<ApiResponse<FooterColumnDto>>> UpdateColumn(int id, [FromBody] FooterColumnUpsertRequest req)
    {
        var col = await db.FooterColumns.Include(c => c.Links).FirstOrDefaultAsync(c => c.Id == id);
        if (col == null) return NotFound(new ApiResponse<FooterColumnDto>(false, "Not found", null));
        col.Heading = req.Heading; col.DisplayOrder = req.DisplayOrder;
        col.IsVisible = req.IsVisible; col.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FooterColumnDto>(true, "Updated", ToColumnDto(col)));
    }

    /// <summary>Delete a footer column and all its links.</summary>
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("columns/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteColumn(int id)
    {
        var col = await db.FooterColumns.FindAsync(id);
        if (col == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.FooterColumns.Remove(col);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    // ── Footer Links ─────────────────────────────────────────────────────────

    /// <summary>Create a footer link.</summary>
    [Authorize]
    [HttpPost("links")]
    public async Task<ActionResult<ApiResponse<FooterLinkDto>>> CreateLink([FromBody] FooterLinkUpsertRequest req)
    {
        var link = new FooterLink { FooterColumnId = req.FooterColumnId, Label = req.Label, Url = req.Url, IsExternal = req.IsExternal, IsVisible = req.IsVisible, DisplayOrder = req.DisplayOrder };
        db.FooterLinks.Add(link);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FooterLinkDto>(true, "Created", ToLinkDto(link)));
    }

    /// <summary>Update a footer link.</summary>
    [Authorize]
    [HttpPut("links/{id}")]
    public async Task<ActionResult<ApiResponse<FooterLinkDto>>> UpdateLink(int id, [FromBody] FooterLinkUpsertRequest req)
    {
        var link = await db.FooterLinks.FindAsync(id);
        if (link == null) return NotFound(new ApiResponse<FooterLinkDto>(false, "Not found", null));
        link.Label = req.Label; link.Url = req.Url; link.IsExternal = req.IsExternal;
        link.IsVisible = req.IsVisible; link.DisplayOrder = req.DisplayOrder;
        link.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FooterLinkDto>(true, "Updated", ToLinkDto(link)));
    }

    /// <summary>Delete a footer link.</summary>
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("links/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteLink(int id)
    {
        var link = await db.FooterLinks.FindAsync(id);
        if (link == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.FooterLinks.Remove(link);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static FooterConfigDto ToConfigDto(FooterConfig c) => new(c.Id,
        c.CompanyDescription, c.CtaLabel, c.CtaUrl, c.CopyrightText,
        c.PrivacyPolicyUrl, c.TermsUrl, c.CookiePolicyUrl, c.UpdatedAt);

    private static FooterColumnDto ToColumnDto(FooterColumn c) => new(c.Id, c.Heading,
        c.DisplayOrder, c.IsVisible, c.Links.Select(ToLinkDto).ToList(), c.UpdatedAt);

    private static FooterLinkDto ToLinkDto(FooterLink l) => new(l.Id, l.FooterColumnId,
        l.Label, l.Url, l.IsExternal, l.IsVisible, l.DisplayOrder);

    private static SiteSettingsDto ToSettingsDto(SiteSettings s) => new(s.Id,
        s.CompanyName, s.LogoUrl, s.Address, s.ContactEmail, s.ContactPhone, s.WhatsAppNumber,
        s.FacebookUrl, s.TwitterUrl, s.InstagramUrl, s.LinkedInUrl, s.YoutubeUrl,
        s.CopyrightText, s.DefaultSeoTitle, s.DefaultSeoDescription, s.UpdatedAt);
}
