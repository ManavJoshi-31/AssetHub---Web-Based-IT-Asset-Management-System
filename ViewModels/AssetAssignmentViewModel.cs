using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class AssetAssignmentViewModel
{
    [Required]
    [Display(Name = "Asset")]
    public int AssetId { get; set; }

    [Required]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [Display(Name = "Assignment Notes")]
    [StringLength(1000)]
    public string? AssignmentNotes { get; set; }
}