using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/hospitals")]
public class HospitalsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HospitalDto>>>> GetPublished(
        [FromQuery] string? country, [FromQuery] string? search, [FromQuery] int? limit)
    {
        var q = db.Hospitals.Where(h => h.Status == "published").AsQueryable();
        if (!string.IsNullOrEmpty(country)) q = q.Where(h => h.Country == country);
        if (!string.IsNullOrEmpty(search)) q = q.Where(h => h.Name.Contains(search) || h.City.Contains(search));
        q = q.OrderBy(h => h.DisplayOrder);
        if (limit.HasValue) q = q.Take(limit.Value);
        var items = await q.Select(h => ToDto(h)).ToListAsync();
        return Ok(new ApiResponse<List<HospitalDto>>(true, "OK", items));
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApiResponse<HospitalDto>>> GetBySlug(string slug)
    {
        var h = await db.Hospitals.FirstOrDefaultAsync(h => h.Slug == slug && h.Status == "published");
        if (h == null) return NotFound(new ApiResponse<HospitalDto>(false, "Not found", null));
        return Ok(new ApiResponse<HospitalDto>(true, "OK", ToDto(h)));
    }

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<PagedResult<HospitalDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await db.Hospitals.CountAsync();
        var items = await db.Hospitals.OrderBy(h => h.DisplayOrder)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(h => ToDto(h)).ToListAsync();
        return Ok(new ApiResponse<PagedResult<HospitalDto>>(true, "OK",
            new PagedResult<HospitalDto>(items, total, page, pageSize)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<HospitalDto>>> Create([FromBody] HospitalUpsertRequest req)
    {
        if (await db.Hospitals.AnyAsync(h => h.Slug == req.Slug))
            return BadRequest(new ApiResponse<HospitalDto>(false, "Slug already exists", null));
        var h = Map(new Hospital(), req);
        db.Hospitals.Add(h);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<HospitalDto>(true, "Created", ToDto(h)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<HospitalDto>>> Update(int id, [FromBody] HospitalUpsertRequest req)
    {
        var h = await db.Hospitals.FindAsync(id);
        if (h == null) return NotFound(new ApiResponse<HospitalDto>(false, "Not found", null));
        if (h.Slug != req.Slug && await db.Hospitals.AnyAsync(x => x.Slug == req.Slug && x.Id != id))
            return BadRequest(new ApiResponse<HospitalDto>(false, "Slug already exists", null));
        Map(h, req); h.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<HospitalDto>(true, "Updated", ToDto(h)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var h = await db.Hospitals.FindAsync(id);
        if (h == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Hospitals.Remove(h);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static Hospital Map(Hospital h, HospitalUpsertRequest r)
    {
        h.Name = r.Name; h.Slug = r.Slug; h.City = r.City; h.Country = r.Country;
        h.Image = r.Image; h.Specialties = r.Specialties; h.Accreditation = r.Accreditation;
        h.Description = r.Description; h.Beds = r.Beds; h.Established = r.Established;
        h.Rating = r.Rating; h.Status = r.Status; h.DisplayOrder = r.DisplayOrder;
        h.SeoTitle = r.SeoTitle; h.SeoDescription = r.SeoDescription;
        return h;
    }

    private static HospitalDto ToDto(Hospital h) => new(h.Id, h.Name, h.Slug, h.City, h.Country,
        h.Image, h.Specialties, h.Accreditation, h.Description, h.Beds, h.Established, h.Rating,
        h.Status, h.DisplayOrder, h.SeoTitle, h.SeoDescription, h.CreatedAt, h.UpdatedAt);
}
