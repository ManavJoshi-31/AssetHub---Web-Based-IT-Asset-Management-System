using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetStatusViewModel
{
    public int AssetId { get; set; }

    public string AssetTag { get; set; } = string.Empty;

    public string AssetName { get; set; } = string.Empty;

    public AssetStatus CurrentStatus { get; set; }

    [Required]
    [Display(Name = "New Status")]
    public AssetStatus NewStatus { get; set; }
}