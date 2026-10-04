using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class MaintenanceViewModel
{
    [Required]
    [Display(Name = "Asset")]
    public int AssetId { get; set; }

    [Required]
    [Display(Name = "Maintenance Type")]
    public MaintenanceType MaintenanceType { get; set; }

    [Display(Name = "Description")]
    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "Vendor")]
    [StringLength(200)]
    public string? Vendor { get; set; }

    [Display(Name = "Cost")]
    [Range(0, 100000000)]
    public decimal? Cost { get; set; }

    [Display(Name = "Technician Name")]
    [StringLength(200)]
    public string? TechnicianName { get; set; }

    [Display(Name = "Notes")]
    [StringLength(2000)]
    public string? Notes { get; set; }
}