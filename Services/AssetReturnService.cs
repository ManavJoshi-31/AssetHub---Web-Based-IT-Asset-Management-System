using AssetHub.Data;
using AssetHub.Models;
using AssetHub.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Services;

public class AssetReturnService
{
    private readonly ApplicationDbContext _context;

    public AssetReturnService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssetAssignment?> GetActiveAssignmentAsync(int assetId)
    {
        return await _context.AssetAssignments
            .FirstOrDefaultAsync(a =>
                a.AssetId == assetId &&
                a.ReturnedAt == null);
    }
    public async Task<(bool Success, string? ErrorMessage)> ReturnAssetAsync(
    int assetId,
    AssetCondition returnCondition,
    string? returnedByUserId,
    string? returnNotes)
    {
        var assignment = await GetActiveAssignmentAsync(assetId);

        if (assignment == null)
        {
            return (false, "No active assignment was found for this asset.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == assetId);

        if (asset == null)
        {
            return (false, "Asset not found.");
        }

        var now = DateTime.UtcNow;

        assignment.ReturnedAt = now;
        assignment.ReturnedByUserId = returnedByUserId;
        assignment.ReturnCondition = returnCondition;
        assignment.ReturnNotes = returnNotes;
        assignment.UpdatedAt = now;

        var newStatus = returnCondition == AssetCondition.Damaged
            ? AssetStatus.Maintenance
            : AssetStatus.Available;

        var oldStatus = asset.AssetStatus;

        asset.AssetStatus = newStatus;
        asset.Condition = returnCondition;
        asset.UpdatedAt = now;

        var statusHistory = new AssetStatusHistory
        {
            AssetId = asset.AssetId,
            OldStatus = oldStatus.ToString(),
            NewStatus = newStatus.ToString(),
            ChangedByUserId = returnedByUserId,
            ChangedAt = now,
            Reason = "Asset returned by employee.",
            Notes = returnNotes
        };

        _context.AssetStatusHistories.Add(statusHistory);

        await _context.SaveChangesAsync();

        return (true, null);
    }
}