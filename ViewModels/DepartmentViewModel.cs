using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class DepartmentViewModel
{
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Department Code")]
    public string DepartmentCode { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? ManagerEmployeeId { get; set; }

    public bool IsActive { get; set; } = true;
}