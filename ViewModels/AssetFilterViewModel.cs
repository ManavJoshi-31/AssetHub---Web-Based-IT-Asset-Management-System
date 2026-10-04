using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetFilterViewModel
{
    [Display(Name = "Search")]
    public string? Search { get; set; }

    [Display(Name = "Category")]
    public int? AssetCategoryId { get; set; }

    [Display(Name = "Status")]
    public AssetStatus? AssetStatus { get; set; }

    [Display(Name = "Condition")]
    public AssetCondition? Condition { get; set; }
}