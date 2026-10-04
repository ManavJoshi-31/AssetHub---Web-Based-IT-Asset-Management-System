using AssetHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetHub.Controllers;

[Authorize]
public class AssetStatusHistoryController : Controller
{
    private readonly AssetStatusHistoryService _statusHistoryService;

    public AssetStatusHistoryController(
        AssetStatusHistoryService statusHistoryService)
    {
        _statusHistoryService = statusHistoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int assetId)
    {
        if (assetId <= 0)
        {
            return BadRequest();
        }

        var history =
            await _statusHistoryService.GetHistoryAsync(assetId);

        ViewBag.AssetId = assetId;

        return View(history);
    }
}