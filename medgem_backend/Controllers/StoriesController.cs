using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/stories")]
public class StoriesController(AppDbContext db, IWebHostEnvironment env, IConfiguration config) : ControllerBase
{
    private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp", "image/gif"];
    private static readonly string[] AllowedVideoTypes = ["video/mp4", "video/webm", "video/ogg"];
    private const long MaxImageSize = 10 * 1024 * 1024;
    private const long MaxVideoSize = 200 * 1024 * 1024;

    // ── Public ────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PatientStoryDto>>>> GetPublished()
    {
        var items = await db.PatientStories
            .Where(s => s.Status == "published")
            .Include(s => s.Photos.OrderBy(p => p.DisplayOrder))
            .OrderBy(s => s.DisplayOrder)
            .Select(s => ToDto(s))
            .ToListAsync();
        return Ok(new ApiResponse<List<PatientStoryDto>>(true, "OK", items));
    }

    [HttpGet("featured")]
    public async Task<ActionResult<ApiResponse<List<PatientStoryDto>>>> GetFeatured()
    {
        var items = await db.PatientStories
            .Where(s => s.Status == "published" && s.IsFeatured)
            .Include(s => s.Photos.OrderBy(p => p.DisplayOrder))
            .OrderBy(s => s.DisplayOrder)
            .Select(s => ToDto(s))
            .ToListAsync();
        return Ok(new ApiResponse<List<PatientStoryDto>>(true, "OK", items));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PatientStoryDto>>> GetById(int id)
    {
        var s = await db.PatientStories
            .Include(s => s.Photos.OrderBy(p => p.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Id == id && s.Status == "published");
        if (s == null) return NotFound(new ApiResponse<PatientStoryDto>(false, "Not found", null));
        return Ok(new ApiResponse<PatientStoryDto>(true, "OK", ToDto(s)));
    }

    // ── Admin ─────────────────────────────────────────────────────────────────

    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<List<PatientStoryDto>>>> GetAll()
    {
        var items = await db.PatientStories
            .Include(s => s.Photos.OrderBy(p => p.DisplayOrder))
            .OrderBy(s => s.DisplayOrder)
            .Select(s => ToDto(s))
            .ToListAsync();
        return Ok(new ApiResponse<List<PatientStoryDto>>(true, "OK", items));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PatientStoryDto>>> Create([FromBody] PatientStoryUpsertRequest req)
    {
        if (!string.IsNullOrWhiteSpace(req.VideoUrl) && req.VideoProvider is "youtube" or "vimeo")
            if (!IsValidExternalVideoUrl(req.VideoUrl, req.VideoProvider))
                return BadRequest(new ApiResponse<PatientStoryDto>(false, "Invalid video URL for provider", null));

        var s = Map(new PatientStory(), req);
        db.PatientStories.Add(s);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientStoryDto>(true, "Created", ToDto(s)));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PatientStoryDto>>> Update(int id, [FromBody] PatientStoryUpsertRequest req)
    {
        var s = await db.PatientStories.Include(s => s.Photos).FirstOrDefaultAsync(s => s.Id == id);
        if (s == null) return NotFound(new ApiResponse<PatientStoryDto>(false, "Not found", null));

        if (!string.IsNullOrWhiteSpace(req.VideoUrl) && req.VideoProvider is "youtube" or "vimeo")
            if (!IsValidExternalVideoUrl(req.VideoUrl, req.VideoProvider))
                return BadRequest(new ApiResponse<PatientStoryDto>(false, "Invalid video URL for provider", null));

        Map(s, req);
        s.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientStoryDto>(true, "Updated", ToDto(s)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var s = await db.PatientStories.Include(s => s.Photos).FirstOrDefaultAsync(s => s.Id == id);
        if (s == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.PatientStories.Remove(s);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    // ── Video Upload ──────────────────────────────────────────────────────────

    [Authorize]
    [HttpPost("{id:int}/video/upload")]
    public async Task<ActionResult<ApiResponse<string>>> UploadVideo(int id, IFormFile file)
    {
        var story = await db.PatientStories.FindAsync(id);
        if (story == null) return NotFound(new ApiResponse<string>(false, "Story not found", null));
        if (file == null || file.Length == 0) return BadRequest(new ApiResponse<string>(false, "No file", null));
        if (file.Length > MaxVideoSize) return BadRequest(new ApiResponse<string>(false, "Video exceeds 200MB limit", null));
        if (!AllowedVideoTypes.Contains(file.ContentType)) return BadRequest(new ApiResponse<string>(false, "Unsupported video format", null));

        var uploadsPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var baseUrl = config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        story.VideoUrl = $"{baseUrl}/uploads/{fileName}";
        story.VideoProvider = "upload";
        story.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<string>(true, "Uploaded", story.VideoUrl));
    }

    // ── Thumbnail Upload ──────────────────────────────────────────────────────

    [Authorize]
    [HttpPost("{id:int}/thumbnail/upload")]
    public async Task<ActionResult<ApiResponse<string>>> UploadThumbnail(int id, IFormFile file)
    {
        var story = await db.PatientStories.FindAsync(id);
        if (story == null) return NotFound(new ApiResponse<string>(false, "Story not found", null));
        if (file == null || file.Length == 0 || file.Length > MaxImageSize || !AllowedImageTypes.Contains(file.ContentType))
            return BadRequest(new ApiResponse<string>(false, "Invalid image file", null));

        var uploadsPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        await using var stream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create);
        await file.CopyToAsync(stream);

        var baseUrl = config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        story.ThumbnailUrl = $"{baseUrl}/uploads/{fileName}";
        story.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<string>(true, "Uploaded", story.ThumbnailUrl));
    }

    // ── Photos ────────────────────────────────────────────────────────────────

    [Authorize]
    [HttpPost("{id:int}/photos/upload")]
    public async Task<ActionResult<ApiResponse<List<PatientStoryPhotoDto>>>> UploadPhotos(int id, [FromForm] IFormFileCollection files)
    {
        var story = await db.PatientStories.Include(s => s.Photos).FirstOrDefaultAsync(s => s.Id == id);
        if (story == null) return NotFound(new ApiResponse<List<PatientStoryPhotoDto>>(false, "Story not found", null));

        var baseUrl = config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var uploadsPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var added = new List<PatientStoryPhoto>();
        foreach (var file in files)
        {
            if (file.Length == 0 || file.Length > MaxImageSize || !AllowedImageTypes.Contains(file.ContentType)) continue;
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            await using var stream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create);
            await file.CopyToAsync(stream);
            added.Add(new PatientStoryPhoto { PatientStoryId = id, Url = $"{baseUrl}/uploads/{fileName}", DisplayOrder = story.Photos.Count + added.Count });
        }
        db.PatientStoryPhotos.AddRange(added);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<List<PatientStoryPhotoDto>>(true, "Uploaded", added.Select(ToPhotoDto).ToList()));
    }

    [Authorize]
    [HttpPost("{id:int}/photos")]
    public async Task<ActionResult<ApiResponse<PatientStoryPhotoDto>>> AddPhotoByUrl(int id, [FromBody] PatientStoryPhotoUpsertRequest req)
    {
        if (!await db.PatientStories.AnyAsync(s => s.Id == id))
            return NotFound(new ApiResponse<PatientStoryPhotoDto>(false, "Story not found", null));
        var p = new PatientStoryPhoto { PatientStoryId = id, Url = req.Url, AltText = req.AltText, Caption = req.Caption, DisplayOrder = req.DisplayOrder };
        db.PatientStoryPhotos.Add(p);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientStoryPhotoDto>(true, "Added", ToPhotoDto(p)));
    }

    [Authorize]
    [HttpPut("photos/{photoId:int}")]
    public async Task<ActionResult<ApiResponse<PatientStoryPhotoDto>>> UpdatePhoto(int photoId, [FromBody] PatientStoryPhotoUpsertRequest req)
    {
        var p = await db.PatientStoryPhotos.FindAsync(photoId);
        if (p == null) return NotFound(new ApiResponse<PatientStoryPhotoDto>(false, "Not found", null));
        p.AltText = req.AltText; p.Caption = req.Caption; p.DisplayOrder = req.DisplayOrder;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<PatientStoryPhotoDto>(true, "Updated", ToPhotoDto(p)));
    }

    [Authorize]
    [HttpDelete("photos/{photoId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePhoto(int photoId)
    {
        var p = await db.PatientStoryPhotos.FindAsync(photoId);
        if (p == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.PatientStoryPhotos.Remove(p);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static bool IsValidExternalVideoUrl(string url, string provider)
    {
        if (provider == "youtube")
            return Regex.IsMatch(url, @"^https?://(www\.)?(youtube\.com/watch\?v=|youtu\.be/)[\w\-]+");
        if (provider == "vimeo")
            return Regex.IsMatch(url, @"^https?://(www\.)?vimeo\.com/\d+");
        return false;
    }

    private static PatientStory Map(PatientStory s, PatientStoryUpsertRequest r)
    {
        s.PatientName = r.PatientName; s.Treatment = r.Treatment; s.Hospital = r.Hospital;
        s.Title = r.Title; s.Description = r.Description; s.ThumbnailUrl = r.ThumbnailUrl;
        s.VideoUrl = r.VideoUrl; s.VideoTitle = r.VideoTitle; s.VideoProvider = r.VideoProvider;
        s.IsFeatured = r.IsFeatured; s.Status = r.Status; s.DisplayOrder = r.DisplayOrder;
        s.ConsentGiven = r.ConsentGiven; s.PublishedAt = r.PublishedAt;
        return s;
    }

    private static PatientStoryDto ToDto(PatientStory s) => new(
        s.Id, s.PatientName, s.Treatment, s.Hospital, s.Title, s.Description,
        s.ThumbnailUrl, s.VideoUrl, s.VideoTitle, s.VideoProvider, s.IsFeatured,
        s.Status, s.DisplayOrder, s.ConsentGiven, s.PublishedAt, s.CreatedAt, s.UpdatedAt,
        s.Photos.Select(ToPhotoDto).ToList());

    private static PatientStoryPhotoDto ToPhotoDto(PatientStoryPhoto p) =>
        new(p.Id, p.PatientStoryId, p.Url, p.AltText, p.Caption, p.DisplayOrder);
}
