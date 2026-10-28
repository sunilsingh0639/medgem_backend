using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

/// <summary>Manages header navigation items (public read, admin write).</summary>
[ApiController]
[Route("api/navigation")]
public class NavigationController(AppDbContext db) : ControllerBase
{
    /// <summary>Get all visible top-level nav items with their children (public).</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PublicNavDto>>> GetPublic()
    {
        var items = await db.NavItems
            .Where(n => n.IsVisible && n.ParentId == null)
            .OrderBy(n => n.DisplayOrder)
            .Include(n => n.Children.Where(c => c.IsVisible).OrderBy(c => c.DisplayOrder))
            .ToListAsync();

        var settings = await db.SiteSettings.FirstOrDefaultAsync() ?? new SiteSettings();
        var dto = new PublicNavDto(items.Select(ToDto).ToList(),
            "Free Consultation", "/consultation");
        return Ok(new ApiResponse<PublicNavDto>(true, "OK", dto));
    }

    /// <summary>Get all nav items (admin, includes hidden).</summary>
    [Authorize]
    [HttpGet("admin/all")]
    public async Task<ActionResult<ApiResponse<List<NavItemDto>>>> GetAll()
    {
        var items = await db.NavItems
            .Where(n => n.ParentId == null)
            .OrderBy(n => n.DisplayOrder)
            .Include(n => n.Children.OrderBy(c => c.DisplayOrder))
            .ToListAsync();
        return Ok(new ApiResponse<List<NavItemDto>>(true, "OK", items.Select(ToDto).ToList()));
    }

    /// <summary>Create a nav item.</summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NavItemDto>>> Create([FromBody] NavItemUpsertRequest req)
    {
        var item = new NavItem { Label = req.Label, Path = req.Path, IsHighlight = req.IsHighlight, IsVisible = req.IsVisible, DisplayOrder = req.DisplayOrder, ParentId = req.ParentId };
        db.NavItems.Add(item);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<NavItemDto>(true, "Created", ToDto(item)));
    }

    /// <summary>Update a nav item.</summary>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<NavItemDto>>> Update(int id, [FromBody] NavItemUpsertRequest req)
    {
        var item = await db.NavItems.FindAsync(id);
        if (item == null) return NotFound(new ApiResponse<NavItemDto>(false, "Not found", null));
        item.Label = req.Label; item.Path = req.Path; item.IsHighlight = req.IsHighlight;
        item.IsVisible = req.IsVisible; item.DisplayOrder = req.DisplayOrder;
        item.ParentId = req.ParentId; item.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<NavItemDto>(true, "Updated", ToDto(item)));
    }

    /// <summary>Delete a nav item (SuperAdmin only).</summary>
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var item = await db.NavItems.Include(n => n.Children).FirstOrDefaultAsync(n => n.Id == id);
        if (item == null) return NotFound(new ApiResponse<object>(false, "Not found", null));
        db.NavItems.RemoveRange(item.Children);
        db.NavItems.Remove(item);
        await db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, "Deleted", null));
    }

    private static NavItemDto ToDto(NavItem n) => new(n.Id, n.Label, n.Path, n.IsHighlight,
        n.IsVisible, n.DisplayOrder, n.ParentId,
        n.Children.Select(c => ToDto(c)).ToList());
}
