using AssetHub.Models.Enums;

namespace AssetHub.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public int DepartmentId { get; set; }

    public string? JobTitle { get; set; }

    public DateTime DateOfJoining { get; set; }

    public DateTime? DateOfLeaving { get; set; }

    public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}