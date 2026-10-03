using AssetHub.Data;
using AssetHub.Models;
using AssetHub.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Services;

public class MaintenanceService
{
    private readonly ApplicationDbContext _context;

    public MaintenanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateMaintenanceAsync(
        int assetId,
        MaintenanceType maintenanceType,
        string? description,
        DateTime startDate,
        string? vendor,
        decimal? cost,
        string? technicianName,
        string? notes)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == assetId);

        if (asset == null)
        {
            return (false, "Asset not found.");
        }

        if (!asset.IsActive)
        {
            return (false, "This asset is inactive and cannot be placed into maintenance.");
        }

        if (asset.AssetStatus != AssetStatus.Available)
        {
            return (false, "Only available assets can be placed into maintenance.");
        }

        var now = DateTime.UtcNow;

        var maintenance = new MaintenanceRecord
        {
            AssetId = assetId,
            MaintenanceType = maintenanceType,
            Description = description,
            StartDate = startDate,
            Status = MaintenanceStatus.Open,
            Vendor = vendor,
            Cost = cost,
            TechnicianName = technicianName,
            Notes = notes,
            CreatedAt = now
        };

        var statusHistory = new AssetStatusHistory
        {
            AssetId = asset.AssetId,
            OldStatus = asset.AssetStatus.ToString(),
            NewStatus = AssetStatus.Maintenance.ToString(),
            ChangedAt = now,
            Reason = "Asset placed into maintenance.",
            Notes = description
        };

        asset.AssetStatus = AssetStatus.Maintenance;
        asset.UpdatedAt = now;

        _context.MaintenanceRecords.Add(maintenance);
        _context.AssetStatusHistories.Add(statusHistory);

        await _context.SaveChangesAsync();

        return (true, null);
    }
    public async Task<(bool Success, string? ErrorMessage)> StartMaintenanceAsync(
    int maintenanceRecordId)
    {
        var maintenance = await _context.MaintenanceRecords
            .FirstOrDefaultAsync(m =>
                m.MaintenanceRecordId == maintenanceRecordId);

        if (maintenance == null)
        {
            return (false, "Maintenance record not found.");
        }

        if (maintenance.Status != MaintenanceStatus.Open)
        {
            return (false, "Only open maintenance records can be started.");
        }

        maintenance.Status = MaintenanceStatus.InProgress;
        maintenance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null);
    }
    public async Task<(bool Success, string? ErrorMessage)> CompleteMaintenanceAsync(
    int maintenanceRecordId)
    {
        var maintenance = await _context.MaintenanceRecords
            .FirstOrDefaultAsync(m =>
                m.MaintenanceRecordId == maintenanceRecordId);

        if (maintenance == null)
        {
            return (false, "Maintenance record not found.");
        }

        if (maintenance.Status != MaintenanceStatus.InProgress)
        {
            return (false, "Only maintenance records in progress can be completed.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == maintenance.AssetId);

        if (asset == null)
        {
            return (false, "Asset not found.");
        }

        var now = DateTime.UtcNow;

        maintenance.Status = MaintenanceStatus.Completed;
        maintenance.EndDate = now;
        maintenance.UpdatedAt = now;

        var oldStatus = asset.AssetStatus;

        asset.AssetStatus = AssetStatus.Available;
        asset.UpdatedAt = now;

        var statusHistory = new AssetStatusHistory
        {
            AssetId = asset.AssetId,
            OldStatus = oldStatus.ToString(),
            NewStatus = AssetStatus.Available.ToString(),
            ChangedAt = now,
            Reason = "Maintenance completed.",
            Notes = maintenance.Notes
        };

        _context.AssetStatusHistories.Add(statusHistory);

        await _context.SaveChangesAsync();

        return (true, null);
    }
    public async Task<(bool Success, string? ErrorMessage)> CancelMaintenanceAsync(
    int maintenanceRecordId)
    {
        var maintenance = await _context.MaintenanceRecords
            .FirstOrDefaultAsync(m =>
                m.MaintenanceRecordId == maintenanceRecordId);

        if (maintenance == null)
        {
            return (false, "Maintenance record not found.");
        }

        if (maintenance.Status != MaintenanceStatus.Open &&
            maintenance.Status != MaintenanceStatus.InProgress)
        {
            return (false, "Only open or in-progress maintenance can be cancelled.");
        }

        maintenance.Status = MaintenanceStatus.Cancelled;
        maintenance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null);
    }
}