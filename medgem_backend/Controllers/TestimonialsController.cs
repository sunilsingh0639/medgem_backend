using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/testimonials")]
public class TestimonialsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TestimonialDto>>>> GetPublished()
    {
        var items = await db.Testimonials.Where(t => t.Status == "published")
            .OrderBy(t => t.DisplayOrder).Select(t => ToDto(t)).ToListAsync();
        return Ok(new ApiResponse<List<TestimonialDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<List<TestimonialDto>>>> GetAll()
    {
        var items = await db.Testimonials.OrderBy(t => t.DisplayOrder).Select(t => ToDto(t)).ToListAsync();
        return Ok(new ApiResponse<List<TestimonialDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TestimonialDto>>> Create([FromBody] TestimonialUpsertRequest req)
    {
        var t = new Testimonial { Name = req.Name, Country = req.Country, Treatment = req.Treatment, Avatar = req.Avatar, Text = req.Text, Rating = req.Rating, Note = req.Note, Status = req.Status, DisplayOrder = req.DisplayOrder };
        db.Testimonials.Add(t);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<TestimonialDto>(true, "Created", ToDto(t)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TestimonialDto>>> Update(int id, [FromBody] TestimonialUpsertRequest req)
    {
        var t = await db.Testimonials.FindAsync(id);
        if (t == null) return NotFound(new ApiResponse<TestimonialDto>(false, "Not found", null));
        t.Name = req.Name; t.Country = req.Country; t.Treatment = req.Treatment; t.Avatar = req.Avatar;
        t.Text = req.Text; t.Rating = req.Rating; t.Note = req.Note; t.Status = req.Status;
        t.DisplayOrder = req.DisplayOrder; t.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<TestimonialDto>(true, "Updated", ToDto(t)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var t = await db.Testimonials.FindAsync(id);
        if (t == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Testimonials.Remove(t);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static TestimonialDto ToDto(Testimonial t) => new(t.Id, t.Name, t.Country, t.Treatment,
        t.Avatar, t.Text, t.Rating, t.Note, t.Status, t.DisplayOrder, t.CreatedAt, t.UpdatedAt);
}
