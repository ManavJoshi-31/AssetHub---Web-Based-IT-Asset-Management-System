using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetViewModel
{
    public int AssetId { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Asset Tag")]
    public string AssetTag { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Asset Name")]
    public string AssetName { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Serial Number")]
    public string? SerialNumber { get; set; }

    [Required]
    [Display(Name = "Asset Category")]
    public int AssetCategoryId { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(100)]
    public string? Model { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Purchase Date")]
    public DateTime PurchaseDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Purchase Cost")]
    public decimal PurchaseCost { get; set; }

    [StringLength(150)]
    public string? Vendor { get; set; }

    [StringLength(100)]
    [Display(Name = "Invoice Number")]
    public string? InvoiceNumber { get; set; }

    [StringLength(200)]
    public string? Location { get; set; }

    [Required]
    [Display(Name = "Condition")]
    public AssetCondition Condition { get; set; } = AssetCondition.New;

    [DataType(DataType.Date)]
    [Display(Name = "Warranty Expiry Date")]
    public DateTime? WarrantyExpiryDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}