using AssetHub.Data;
using AssetHub.Models;
using Microsoft.EntityFrameworkCore;
using AssetHub.Models.Enums;
namespace AssetHub.Services;

public class WarrantyService
{
    private readonly ApplicationDbContext _context;

    public WarrantyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateWarrantyAsync(
        Warranty warranty)
    {
        if (warranty.EndDate <= warranty.StartDate)
        {
            return (false, "Warranty end date must be after the start date.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == warranty.AssetId);

        if (asset == null)
        {
            return (false, "Asset not found.");
        }

        var existingWarranty = await _context.Warranties
            .AnyAsync(w => w.AssetId == warranty.AssetId);

        if (existingWarranty)
        {
            return (false, "This asset already has a warranty.");
        }

        var now = DateTime.UtcNow;

        warranty.CreatedAt = now;
        warranty.UpdatedAt = null;

        _context.Warranties.Add(warranty);

        await _context.SaveChangesAsync();

        return (true, null);
    }
    public WarrantyStatus GetWarrantyStatus(DateTime endDate)
    {
        var today = DateTime.Today;

        if (endDate.Date < today)
        {
            return WarrantyStatus.Expired;
        }

        if (endDate.Date <= today.AddDays(30))
        {
            return WarrantyStatus.ExpiringSoon;
        }

        return WarrantyStatus.Active;
    }
}