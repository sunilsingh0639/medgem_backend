namespace medgem_backend.Models;

/// <summary>Header navigation item — supports one level of dropdown children.</summary>
public class NavItem
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool IsHighlight { get; set; } = false;
    public bool IsVisible { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    /// <summary>Null = top-level item. Set to parent Id for dropdown children.</summary>
    public int? ParentId { get; set; }
    public NavItem? Parent { get; set; }
    public List<NavItem> Children { get; set; } = [];
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
