namespace AssetHub.Models;

public class AssetCategory
{
    public int AssetCategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string CategoryCode { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}