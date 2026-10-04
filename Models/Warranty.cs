using AssetHub.Models.Enums;

namespace AssetHub.Models;

public class Warranty
{
    public int WarrantyId { get; set; }

    public int AssetId { get; set; }

    public WarrantyType WarrantyType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Provider { get; set; }

    public string? WarrantyNumber { get; set; }

    public string? TermsAndConditions { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}