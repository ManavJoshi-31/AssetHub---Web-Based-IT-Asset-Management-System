using AssetHub.Models.Enums;

namespace AssetHub.ViewModels;

public class EmployeeListViewModel
{
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? JobTitle { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public DateTime DateOfJoining { get; set; }

    public EmploymentStatus EmploymentStatus { get; set; }

    public bool IsActive { get; set; }
}