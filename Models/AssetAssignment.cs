namespace AssetHub.Models;

public class AssetAssignment
{
    public int AssetAssignmentId { get; set; }

    public int AssetId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime AssignedAt { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public string? AssignedByUserId { get; set; }

    public string? ReturnedByUserId { get; set; }

    public string? AssignmentNotes { get; set; }

    public string? ReturnNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}