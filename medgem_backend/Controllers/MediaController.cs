using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/media")]
[Authorize]
public class MediaController(AppDbContext db, IWebHostEnvironment env, IConfiguration config) : ControllerBase
{
    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp", "image/gif", "application/pdf"];
    private const long MaxSize = 10 * 1024 * 1024; // 10MB

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<MediaAssetDto>>>> GetAll()
    {
        var items = await db.MediaAssets.OrderByDescending(m => m.CreatedAt)
            .Select(m => ToDto(m)).ToListAsync();
        return Ok(new ApiResponse<List<MediaAssetDto>>(true, "OK", items));
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ApiResponse<MediaAssetDto>>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new ApiResponse<MediaAssetDto>(false, "No file provided", null));
        if (file.Length > MaxSize)
            return BadRequest(new ApiResponse<MediaAssetDto>(false, "File exceeds 10MB limit", null));
        if (!AllowedTypes.Contains(file.ContentType))
            return BadRequest(new ApiResponse<MediaAssetDto>(false, "File type not allowed", null));

        var uploadsPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        var baseUrl = config["App:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var asset = new MediaAsset
        {
            FileName = fileName,
            OriginalName = file.FileName,
            Url = $"{baseUrl}/uploads/{fileName}",
            MimeType = file.ContentType,
            FileSize = file.Length
        };
        db.MediaAssets.Add(asset);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<MediaAssetDto>(true, "Uploaded", ToDto(asset)));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MediaAssetDto>>> UpdateMeta(int id,
        [FromBody] MediaAssetDto req)
    {
        var m = await db.MediaAssets.FindAsync(id);
        if (m == null) return NotFound(new ApiResponse<MediaAssetDto>(false, "Not found", null));
        m.AltText = req.AltText; m.Caption = req.Caption;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<MediaAssetDto>(true, "Updated", ToDto(m)));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var m = await db.MediaAssets.FindAsync(id);
        if (m == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        var filePath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads", m.FileName);
        if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        db.MediaAssets.Remove(m);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static MediaAssetDto ToDto(MediaAsset m) => new(m.Id, m.FileName, m.OriginalName,
        m.Url, m.AltText, m.Caption, m.MimeType, m.FileSize, m.CreatedAt);
}
