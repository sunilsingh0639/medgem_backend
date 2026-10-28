using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DoctorDto>>>> GetPublished(
        [FromQuery] string? specialty, [FromQuery] string? country, [FromQuery] string? search, [FromQuery] int? limit)
    {
        var q = db.Doctors.Where(d => d.Status == "published").AsQueryable();
        if (!string.IsNullOrEmpty(specialty) && specialty != "All") q = q.Where(d => d.Specialty.Contains(specialty));
        if (!string.IsNullOrEmpty(country)) q = q.Where(d => d.Country == country);
        if (!string.IsNullOrEmpty(search)) q = q.Where(d => d.Name.Contains(search) || d.Specialty.Contains(search));
        q = q.OrderBy(d => d.DisplayOrder);
        if (limit.HasValue) q = q.Take(limit.Value);
        var items = await q.Select(d => ToDto(d)).ToListAsync();
        return Ok(new ApiResponse<List<DoctorDto>>(true, "OK", items));
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> GetBySlug(string slug)
    {
        var d = await db.Doctors.FirstOrDefaultAsync(d => d.Slug == slug && d.Status == "published");
        if (d == null) return NotFound(new ApiResponse<DoctorDto>(false, "Not found", null));
        return Ok(new ApiResponse<DoctorDto>(true, "OK", ToDto(d)));
    }

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<PagedResult<DoctorDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await db.Doctors.CountAsync();
        var items = await db.Doctors.OrderBy(d => d.DisplayOrder)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(d => ToDto(d)).ToListAsync();
        return Ok(new ApiResponse<PagedResult<DoctorDto>>(true, "OK",
            new PagedResult<DoctorDto>(items, total, page, pageSize)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Create([FromBody] DoctorUpsertRequest req)
    {
        if (await db.Doctors.AnyAsync(d => d.Slug == req.Slug))
            return BadRequest(new ApiResponse<DoctorDto>(false, "Slug already exists", null));
        var d = Map(new Doctor(), req);
        db.Doctors.Add(d);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<DoctorDto>(true, "Created", ToDto(d)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Update(int id, [FromBody] DoctorUpsertRequest req)
    {
        var d = await db.Doctors.FindAsync(id);
        if (d == null) return NotFound(new ApiResponse<DoctorDto>(false, "Not found", null));
        if (d.Slug != req.Slug && await db.Doctors.AnyAsync(x => x.Slug == req.Slug && x.Id != id))
            return BadRequest(new ApiResponse<DoctorDto>(false, "Slug already exists", null));
        Map(d, req); d.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<DoctorDto>(true, "Updated", ToDto(d)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var d = await db.Doctors.FindAsync(id);
        if (d == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Doctors.Remove(d);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static Doctor Map(Doctor d, DoctorUpsertRequest r)
    {
        d.Name = r.Name; d.Slug = r.Slug; d.Specialty = r.Specialty; d.Hospital = r.Hospital;
        d.Country = r.Country; d.Experience = r.Experience; d.Languages = r.Languages;
        d.Image = r.Image; d.Education = r.Education; d.About = r.About; d.Tags = r.Tags;
        d.Status = r.Status; d.DisplayOrder = r.DisplayOrder;
        d.SeoTitle = r.SeoTitle; d.SeoDescription = r.SeoDescription;
        return d;
    }

    private static DoctorDto ToDto(Doctor d) => new(d.Id, d.Name, d.Slug, d.Specialty, d.Hospital,
        d.Country, d.Experience, d.Languages, d.Image, d.Education, d.About, d.Tags,
        d.Status, d.DisplayOrder, d.SeoTitle, d.SeoDescription, d.CreatedAt, d.UpdatedAt);
}
