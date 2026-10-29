using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/gallery")]
public class GalleryController(AppDbContext db, IWebHostEnvironment env, IConfiguration config) : ControllerBase
{
    private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp", "image/gif"];
    private const long MaxImageSize = 10 * 1024 * 1024;

    // ── Public ────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PatientGalleryDto>>>> GetPublished()
    {
        var items = await db.PatientGalleries
            .Where(g => g.Status == "published")
            .Include(g => g.Images.OrderBy(i => i.DisplayOrder))
            .OrderBy(g => g.DisplayOrder)
            .Select(g => ToDto(g))
            .ToListAsync();
        return Ok(new ApiResponse<List<PatientGalleryDto>>(true, "OK", items));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PatientGalleryDto>>> GetById(int id)
    {
        var g = await db.PatientGalleries
            .Include(g => g.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(g => g.Id == id && g.Status == "published");
        if (g == null) return NotFound(new ApiResponse<PatientGalleryDto>(false, "Not found", null));
        return Ok(new ApiResponse<PatientGalleryDto>(true, "OK", ToDto(g)));
    }

    // ── Admin ─────────────────────────────────────────────────────────────────

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<List<PatientGalleryDto>>>> GetAll()
    {
        var items = await db.PatientGalleries
            .Include(g => g.Images.OrderBy(i => i.DisplayOrder))
            .OrderBy(g => g.DisplayOrder)
            .Select(g => ToDto(g))
            .ToListAsync();
        return Ok(new ApiResponse<List<PatientGalleryDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PatientGalleryDto>>> Create([FromBody] PatientGalleryUpsertRequest req)
    {
        var g = Map(new PatientGallery(), req);
        db.PatientGalleries.Add(g);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientGalleryDto>(true, "Created", ToDto(g)));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PatientGalleryDto>>> Update(int id, [FromBody] PatientGalleryUpsertRequest req)
    {
        var g = await db.PatientGalleries.Include(g => g.Images).FirstOrDefaultAsync(g => g.Id == id);
        if (g == null) return NotFound(new ApiResponse<PatientGalleryDto>(false, "Not found", null));
        Map(g, req);
        g.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientGalleryDto>(true, "Updated", ToDto(g)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var g = await db.PatientGalleries.Include(g => g.Images).FirstOrDefaultAsync(g => g.Id == id);
        if (g == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.PatientGalleries.Remove(g);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    // ── Images ────────────────────────────────────────────────────────────────

    [Authorize]
    [HttpPost("{id:int}/images/upload")]
    public async Task<ActionResult<ApiResponse<List<PatientGalleryImageDto>>>> UploadImages(int id, [FromForm] IFormFileCollection files)
    {
        var gallery = await db.PatientGalleries.Include(g => g.Images).FirstOrDefaultAsync(g => g.Id == id);
        if (gallery == null) return NotFound(new ApiResponse<List<PatientGalleryImageDto>>(false, "Gallery not found", null));

        var baseUrl = config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var uploadsPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var added = new List<PatientGalleryImage>();
        foreach (var file in files)
        {
            if (file.Length == 0 || file.Length > MaxImageSize || !AllowedImageTypes.Contains(file.ContentType))
                continue;
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            var img = new PatientGalleryImage
            {
                PatientGalleryId = id,
                Url = $"{baseUrl}/uploads/{fileName}",
                DisplayOrder = gallery.Images.Count + added.Count
            };
            added.Add(img);
        }
        db.PatientGalleryImages.AddRange(added);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<List<PatientGalleryImageDto>>(true, "Uploaded", added.Select(ToImageDto).ToList()));
    }

    [Authorize]
    [HttpPost("{id:int}/images")]
    public async Task<ActionResult<ApiResponse<PatientGalleryImageDto>>> AddImageByUrl(int id, [FromBody] PatientGalleryImageUpsertRequest req)
    {
        if (!await db.PatientGalleries.AnyAsync(g => g.Id == id))
            return NotFound(new ApiResponse<PatientGalleryImageDto>(false, "Gallery not found", null));
        var img = new PatientGalleryImage { PatientGalleryId = id, Url = req.Url, AltText = req.AltText, Caption = req.Caption, DisplayOrder = req.DisplayOrder };
        db.PatientGalleryImages.Add(img);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientGalleryImageDto>(true, "Added", ToImageDto(img)));
    }

    [Authorize]
    [HttpPut("images/{imgId:int}")]
    public async Task<ActionResult<ApiResponse<PatientGalleryImageDto>>> UpdateImage(int imgId, [FromBody] PatientGalleryImageUpsertRequest req)
    {
        var img = await db.PatientGalleryImages.FindAsync(imgId);
        if (img == null) return NotFound(new ApiResponse<PatientGalleryImageDto>(false, "Not found", null));
        img.AltText = req.AltText; img.Caption = req.Caption; img.DisplayOrder = req.DisplayOrder;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientGalleryImageDto>(true, "Updated", ToImageDto(img)));
    }

    [Authorize]
    [HttpDelete("images/{imgId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteImage(int imgId)
    {
        var img = await db.PatientGalleryImages.FindAsync(imgId);
        if (img == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.PatientGalleryImages.Remove(img);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static PatientGallery Map(PatientGallery g, PatientGalleryUpsertRequest r)
    {
        g.PatientName = r.PatientName; g.TreatmentName = r.TreatmentName; g.Title = r.Title;
        g.Description = r.Description; g.Location = r.Location; g.Status = r.Status;
        g.DisplayOrder = r.DisplayOrder; g.ConsentGiven = r.ConsentGiven;
        return g;
    }

    private static PatientGalleryDto ToDto(PatientGallery g) => new(
        g.Id, g.PatientName, g.TreatmentName, g.Title, g.Description, g.Location,
        g.Status, g.DisplayOrder, g.ConsentGiven, g.CreatedAt, g.UpdatedAt,
        g.Images.Select(ToImageDto).ToList());

    private static PatientGalleryImageDto ToImageDto(PatientGalleryImage i) =>
        new(i.Id, i.PatientGalleryId, i.Url, i.AltText, i.Caption, i.DisplayOrder);
}
