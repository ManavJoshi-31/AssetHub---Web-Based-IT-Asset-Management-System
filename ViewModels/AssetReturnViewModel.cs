using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetReturnViewModel
{
    [Required]
    [Display(Name = "Asset")]
    public int AssetId { get; set; }

    [Required]
    [Display(Name = "Return Condition")]
    public AssetCondition ReturnCondition { get; set; }

    [Display(Name = "Return Notes")]
    [StringLength(1000)]
    public string? ReturnNotes { get; set; }
}