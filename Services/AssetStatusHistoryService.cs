using AssetHub.Data;
using AssetHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Services;

public class AssetStatusHistoryService
{
    private readonly ApplicationDbContext _context;

    public AssetStatusHistoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AssetStatusHistory>> GetHistoryAsync(int assetId)
    {
        return await _context.AssetStatusHistories
            .Where(h => h.AssetId == assetId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }
}