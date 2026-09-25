using AssetHub.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Employee Code")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    [StringLength(100)]
    [Display(Name = "Job Title")]
    public string? JobTitle { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Joining")]
    public DateTime DateOfJoining { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Leaving")]
    public DateTime? DateOfLeaving { get; set; }

    [Required]
    [Display(Name = "Employment Status")]
    public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;

    [StringLength(500)]
    public string? Address { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}