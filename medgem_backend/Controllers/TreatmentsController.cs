using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/treatments")]
public class TreatmentsController(AppDbContext db) : ControllerBase
{
    // Public
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TreatmentDto>>>> GetPublished(
        [FromQuery] string? category, [FromQuery] string? search)
    {
        var q = db.Treatments.Where(t => t.Status == "published").AsQueryable();
        if (!string.IsNullOrEmpty(category) && category != "All")
            q = q.Where(t => t.Category == category);
        if (!string.IsNullOrEmpty(search))
            q = q.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
        var items = await q.OrderBy(t => t.DisplayOrder).Select(t => ToDto(t)).ToListAsync();
        return Ok(new ApiResponse<List<TreatmentDto>>(true, "OK", items));
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApiResponse<TreatmentDto>>> GetBySlug(string slug)
    {
        var t = await db.Treatments.FirstOrDefaultAsync(t => t.Slug == slug && t.Status == "published");
        if (t == null) return NotFound(new ApiResponse<TreatmentDto>(false, "Not found", null));
        return Ok(new ApiResponse<TreatmentDto>(true, "OK", ToDto(t)));
    }

    // Admin
    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<PagedResult<TreatmentDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await db.Treatments.CountAsync();
        var items = await db.Treatments.OrderBy(t => t.DisplayOrder)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(t => ToDto(t)).ToListAsync();
        return Ok(new ApiResponse<PagedResult<TreatmentDto>>(true, "OK",
            new PagedResult<TreatmentDto>(items, total, page, pageSize)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TreatmentDto>>> Create([FromBody] TreatmentUpsertRequest req)
    {
        if (await db.Treatments.AnyAsync(t => t.Slug == req.Slug))
            return BadRequest(new ApiResponse<TreatmentDto>(false, "Slug already exists", null));
        var t = new Treatment { Title = req.Title, Slug = req.Slug, Category = req.Category, Description = req.Description, Image = req.Image, Icon = req.Icon, Duration = req.Duration, Destinations = req.Destinations, Tags = req.Tags, Status = req.Status, DisplayOrder = req.DisplayOrder, SeoTitle = req.SeoTitle, SeoDescription = req.SeoDescription };
        db.Treatments.Add(t);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<TreatmentDto>(true, "Created", ToDto(t)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TreatmentDto>>> Update(int id, [FromBody] TreatmentUpsertRequest req)
    {
        var t = await db.Treatments.FindAsync(id);
        if (t == null) return NotFound(new ApiResponse<TreatmentDto>(false, "Not found", null));
        if (t.Slug != req.Slug && await db.Treatments.AnyAsync(x => x.Slug == req.Slug && x.Id != id))
            return BadRequest(new ApiResponse<TreatmentDto>(false, "Slug already exists", null));
        t.Title = req.Title; t.Slug = req.Slug; t.Category = req.Category; t.Description = req.Description;
        t.Image = req.Image; t.Icon = req.Icon; t.Duration = req.Duration; t.Destinations = req.Destinations;
        t.Tags = req.Tags; t.Status = req.Status; t.DisplayOrder = req.DisplayOrder;
        t.SeoTitle = req.SeoTitle; t.SeoDescription = req.SeoDescription; t.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<TreatmentDto>(true, "Updated", ToDto(t)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var t = await db.Treatments.FindAsync(id);
        if (t == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Treatments.Remove(t);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static TreatmentDto ToDto(Treatment t) => new(t.Id, t.Title, t.Slug, t.Category,
        t.Description, t.Image, t.Icon, t.Duration, t.Destinations, t.Tags,
        t.Status, t.DisplayOrder, t.SeoTitle, t.SeoDescription, t.CreatedAt, t.UpdatedAt);
}
