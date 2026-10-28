using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/enquiries")]
public class EnquiriesController(AppDbContext db) : ControllerBase
{
    [HttpPost("contact")]
    public async Task<ActionResult<ApiResponse<object>>> SubmitContact([FromBody] ContactSubmitRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Phone))
            return BadRequest(new ApiResponse<object>(false, "Name, email and phone are required", null));
        db.Enquiries.Add(new Enquiry { Name = req.Name, Email = req.Email, PhoneCode = req.PhoneCode, Phone = req.Phone, Treatment = req.Treatment, Destination = req.Destination, Message = req.Message, Type = "contact" });
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Enquiry received. Our team will be in touch shortly.", null));
    }

    [HttpPost("consultation")]
    public async Task<ActionResult<ApiResponse<object>>> SubmitConsultation([FromBody] ConsultationSubmitRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Phone))
            return BadRequest(new ApiResponse<object>(false, "Name, email and phone are required", null));
        db.Enquiries.Add(new Enquiry { Name = req.Name, Email = req.Email, Phone = req.Phone, Country = req.Country, Treatment = req.Treatment, Destination = req.Destination, Description = req.Description, Type = "consultation" });
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Consultation request received. Our team will be in touch shortly.", null));
    }

    [HttpPost("second-opinion")]
    public async Task<ActionResult<ApiResponse<object>>> SubmitSecondOpinion([FromBody] SecondOpinionSubmitRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Email))
            return BadRequest(new ApiResponse<object>(false, "Name and email are required", null));
        db.Enquiries.Add(new Enquiry { Name = req.Name, Email = req.Email, Phone = req.Phone, Treatment = req.Treatment, Diagnosis = req.Diagnosis, Description = req.Description, Type = "second-opinion" });
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Second opinion request received.", null));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<EnquiryDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null, [FromQuery] string? type = null,
        [FromQuery] string? search = null)
    {
        var q = db.Enquiries.AsQueryable();
        if (!string.IsNullOrEmpty(status)) q = q.Where(e => e.Status == status);
        if (!string.IsNullOrEmpty(type)) q = q.Where(e => e.Type == type);
        if (!string.IsNullOrEmpty(search)) q = q.Where(e => e.Name.Contains(search) || e.Email.Contains(search));
        var total = await q.CountAsync();
        var items = await q.OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).Select(e => ToDto(e)).ToListAsync();
        return Ok(new ApiResponse<PagedResult<EnquiryDto>>(true, "OK",
            new PagedResult<EnquiryDto>(items, total, page, pageSize)));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EnquiryDto>>> GetById(int id)
    {
        var e = await db.Enquiries.FindAsync(id);
        if (e == null) return NotFound(new ApiResponse<EnquiryDto>(false, "Not found", null));
        return Ok(new ApiResponse<EnquiryDto>(true, "OK", ToDto(e)));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<EnquiryDto>>> Update(int id, [FromBody] EnquiryUpdateRequest req)
    {
        var e = await db.Enquiries.FindAsync(id);
        if (e == null) return NotFound(new ApiResponse<EnquiryDto>(false, "Not found", null));
        e.Status = req.Status; e.InternalNotes = req.InternalNotes; e.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<EnquiryDto>(true, "Updated", ToDto(e)));
    }

    private static EnquiryDto ToDto(Enquiry e) => new(e.Id, e.Name, e.Email, e.PhoneCode, e.Phone,
        e.Treatment, e.Destination, e.Message, e.Type, e.Status, e.InternalNotes,
        e.Country, e.Description, e.Diagnosis, e.CreatedAt, e.UpdatedAt);
}
