using AssetHub.Data;
using AssetHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class AssetStatusHistoryController : Controller
{
    private readonly AssetStatusHistoryService _statusHistoryService;
    private readonly ApplicationDbContext _context;

    public AssetStatusHistoryController(
        AssetStatusHistoryService statusHistoryService,
        ApplicationDbContext context)
    {
        _statusHistoryService = statusHistoryService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int assetId)
    {
        if (assetId <= 0)
            return BadRequest();

        var asset = await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AssetId == assetId);

        if (asset == null)
            return NotFound();

        var history = await _statusHistoryService
            .GetHistoryAsync(assetId);

        var userIds = history
            .Where(h => !string.IsNullOrWhiteSpace(h.ChangedByUserId))
            .Select(h => h.ChangedByUserId!)
            .Distinct()
            .ToList();

        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(
                u => u.Id,
                u => !string.IsNullOrWhiteSpace($"{u.FirstName} {u.LastName}".Trim())
                    ? $"{u.FirstName} {u.LastName}".Trim()
                    : (u.Email ?? u.UserName ?? "User"));

        ViewBag.Asset = asset;
        ViewBag.AssetId = assetId;
        ViewBag.UserNames = users;

        return View(history);
    }
}