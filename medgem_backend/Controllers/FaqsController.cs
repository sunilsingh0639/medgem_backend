using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/faqs")]
public class FaqsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<FaqDto>>>> GetPublished()
    {
        var items = await db.Faqs.Where(f => f.Status == "published")
            .OrderBy(f => f.DisplayOrder).Select(f => ToDto(f)).ToListAsync();
        return Ok(new ApiResponse<List<FaqDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<List<FaqDto>>>> GetAll()
    {
        var items = await db.Faqs.OrderBy(f => f.DisplayOrder).Select(f => ToDto(f)).ToListAsync();
        return Ok(new ApiResponse<List<FaqDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FaqDto>>> Create([FromBody] FaqUpsertRequest req)
    {
        var f = new Faq { Question = req.Question, Answer = req.Answer, Category = req.Category, Status = req.Status, DisplayOrder = req.DisplayOrder };
        db.Faqs.Add(f);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FaqDto>(true, "Created", ToDto(f)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<FaqDto>>> Update(int id, [FromBody] FaqUpsertRequest req)
    {
        var f = await db.Faqs.FindAsync(id);
        if (f == null) return NotFound(new ApiResponse<FaqDto>(false, "Not found", null));
        f.Question = req.Question; f.Answer = req.Answer; f.Category = req.Category;
        f.Status = req.Status; f.DisplayOrder = req.DisplayOrder; f.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<FaqDto>(true, "Updated", ToDto(f)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var f = await db.Faqs.FindAsync(id);
        if (f == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.Faqs.Remove(f);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static FaqDto ToDto(Faq f) => new(f.Id, f.Question, f.Answer, f.Category,
        f.Status, f.DisplayOrder, f.CreatedAt, f.UpdatedAt);
}
