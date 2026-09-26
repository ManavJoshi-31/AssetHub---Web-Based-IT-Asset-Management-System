using AssetHub.Models.Enums;

namespace AssetHub.ViewModels;

public class AssetListViewModel
{
    public int AssetId { get; set; }

    public string AssetTag { get; set; } = string.Empty;

    public string AssetName { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public AssetStatus AssetStatus { get; set; }

    public AssetCondition Condition { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; }
}