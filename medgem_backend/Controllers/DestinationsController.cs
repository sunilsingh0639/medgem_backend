using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/destinations")]
public class DestinationsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DestinationDto>>>> GetPublished()
    {
        var items = await db.Destinations.Where(d => d.Status == "published")
            .OrderBy(d => d.DisplayOrder).Select(d => ToDto(d)).ToListAsync();
        return Ok(new ApiResponse<List<DestinationDto>>(true, "OK", items));
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApiResponse<DestinationDto>>> GetBySlug(string slug)
    {
        var d = await db.Destinations.FirstOrDefaultAsync(d => d.Slug == slug && d.Status == "published");
        if (d == null) return NotFound(new ApiResponse<DestinationDto>(false, "Not found", null));
        return Ok(new ApiResponse<DestinationDto>(true, "OK", ToDto(d)));
    }

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<PagedResult<DestinationDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await db.Destinations.CountAsync();
        var items = await db.Destinations.OrderBy(d => d.DisplayOrder)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(d => ToDto(d)).ToListAsync();
        return Ok(new ApiResponse<PagedResult<DestinationDto>>(true, "OK",
            new PagedResult<DestinationDto>(items, total, page, pageSize)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DestinationDto>>> Create([FromBody] DestinationUpsertRequest req)
    {
        if (await db.Destinations.AnyAsync(d => d.Slug == req.Slug))
            return BadRequest(new ApiResponse<DestinationDto>(false, "Slug already exists", null));
        var d = Map(new Destination(), req);
        db.Destinations.Add(d);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<DestinationDto>(true, "Created", ToDto(d)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DestinationDto>>> Update(int id, [FromBody] DestinationUpsertRequest req)
    {
        var d = await db.Destinations.FindAsync(id);
        if (d == null) return NotFound(new ApiResponse<DestinationDto>(false, "Not found", null));
        if (d.Slug != req.Slug && await db.Destinations.AnyAsync(x => x.Slug == req.Slug && x.Id != id))
            return BadRequest(new ApiResponse<DestinationDto>(false, "Slug already exists", null));
        Map(d, req); d.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<DestinationDto>(true, "Updated", ToDto(d)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var d = await db.Destinations.FindAsync(id);
        if (d == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Destinations.Remove(d);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static Destination Map(Destination d, DestinationUpsertRequest r)
    {
        d.Country = r.Country; d.City = r.City; d.Slug = r.Slug; d.Tagline = r.Tagline;
        d.Description = r.Description; d.Image = r.Image; d.Treatments = r.Treatments;
        d.Hospitals = r.Hospitals; d.Specialists = r.Specialists; d.Flag = r.Flag;
        d.Highlights = r.Highlights; d.Status = r.Status; d.DisplayOrder = r.DisplayOrder;
        d.SeoTitle = r.SeoTitle; d.SeoDescription = r.SeoDescription;
        return d;
    }

    private static DestinationDto ToDto(Destination d) => new(d.Id, d.Country, d.City, d.Slug,
        d.Tagline, d.Description, d.Image, d.Treatments, d.Hospitals, d.Specialists,
        d.Flag, d.Highlights, d.Status, d.DisplayOrder, d.SeoTitle, d.SeoDescription,
        d.CreatedAt, d.UpdatedAt);
}
