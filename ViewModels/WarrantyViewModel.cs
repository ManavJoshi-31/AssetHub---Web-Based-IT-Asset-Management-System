using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class WarrantyViewModel
{
    [Required]
    [Display(Name = "Asset")]
    public int AssetId { get; set; }

    [Required]
    [Display(Name = "Warranty Type")]
    public WarrantyType WarrantyType { get; set; }

    [Required]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Display(Name = "Provider")]
    [StringLength(200)]
    public string? Provider { get; set; }

    [Display(Name = "Warranty Number")]
    [StringLength(200)]
    public string? WarrantyNumber { get; set; }

    [Display(Name = "Terms and Conditions")]
    [StringLength(2000)]
    public string? TermsAndConditions { get; set; }

    [Display(Name = "Notes")]
    [StringLength(1000)]
    public string? Notes { get; set; }
}