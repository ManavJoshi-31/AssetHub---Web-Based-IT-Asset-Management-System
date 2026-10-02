using AssetHub.Data;
using AssetHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Services;

public class AssetAssignmentService
{
    private readonly ApplicationDbContext _context;

    public AssetAssignmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? ErrorMessage)> AssignAssetAsync(
        int assetId,
        int employeeId,
        string? assignedByUserId,
        string? assignmentNotes)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
        {
            return (false, "Employee not found.");
        }

        if (!employee.IsActive)
        {
            return (false, "Only active employees can receive assets.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == assetId);

        if (asset == null)
        {
            return (false, "Asset not found.");
        }

        if (!asset.IsActive)
        {
            return (false, "This asset is inactive and cannot be assigned.");
        }

        if (asset.AssetStatus != Models.Enums.AssetStatus.Available)
        {
            return (false, "Only available assets can be assigned.");
        }

        var existingAssignment = await _context.AssetAssignments
            .AnyAsync(a =>
                a.AssetId == assetId &&
                a.ReturnedAt == null);

        if (existingAssignment)
        {
            return (false, "This asset already has an active assignment.");
        }

        var now = DateTime.UtcNow;

        var assignment = new AssetAssignment
        {
            AssetId = assetId,
            EmployeeId = employeeId,
            AssignedAt = now,
            AssignedByUserId = assignedByUserId,
            AssignmentNotes = assignmentNotes,
            CreatedAt = now
        };

        asset.AssetStatus = Models.Enums.AssetStatus.Assigned;
        asset.UpdatedAt = now;

        var statusHistory = new AssetStatusHistory
        {
            AssetId = asset.AssetId,
            OldStatus = Models.Enums.AssetStatus.Available.ToString(),
            NewStatus = Models.Enums.AssetStatus.Assigned.ToString(),
            ChangedByUserId = assignedByUserId,
            ChangedAt = now,
            Reason = "Asset assigned to employee.",
            Notes = assignmentNotes
        };

        _context.AssetAssignments.Add(assignment);
        _context.AssetStatusHistories.Add(statusHistory);

        await _context.SaveChangesAsync();

        return (true, null);
    }
}