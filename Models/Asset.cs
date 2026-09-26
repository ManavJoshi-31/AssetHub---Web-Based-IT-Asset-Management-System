using AssetHub.Models.Enums;

namespace AssetHub.Models;

public class Asset
{
    public int AssetId { get; set; }

    public string AssetTag { get; set; } = string.Empty;

    public string AssetName { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }

    public int AssetCategoryId { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? Description { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal PurchaseCost { get; set; }

    public string? Vendor { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? Location { get; set; }

    public AssetStatus AssetStatus { get; set; } = AssetStatus.Available;

    public AssetCondition Condition { get; set; } = AssetCondition.New;

    public DateTime? WarrantyExpiryDate { get; set; }

    public DateTime? RetirementDate { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}