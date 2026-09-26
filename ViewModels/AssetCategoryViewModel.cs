using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetCategoryViewModel
{
    public int AssetCategoryId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Category Code")]
    public string CategoryCode { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}