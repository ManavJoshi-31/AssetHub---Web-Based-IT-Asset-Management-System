using AssetHub.Data;
using AssetHub.Services;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetHub.Controllers;

[Authorize]
public class AssetReturnController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AssetReturnService _returnService;

    public AssetReturnController(
        ApplicationDbContext context,
        AssetReturnService returnService)
    {
        _context = context;
        _returnService = returnService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadAssetsAsync();

        return View(new AssetReturnViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssetReturnViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadAssetsAsync();
            return View(model);
        }

        var returnedByUserId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _returnService.ReturnAssetAsync(
            model.AssetId,
            model.ReturnCondition,
            returnedByUserId,
            model.ReturnNotes);

        if (!result.Success)
        {
            ModelState.AddModelError(
                string.Empty,
                result.ErrorMessage!);

            await LoadAssetsAsync();
            return View(model);
        }

        TempData["SuccessMessage"] = "Asset returned successfully.";

        return RedirectToAction(nameof(Create));
    }

    private async Task LoadAssetsAsync()
    {
        var assets = await _context.Assets
            .Where(a => a.IsActive &&
                        a.AssetStatus == Models.Enums.AssetStatus.Assigned)
            .OrderBy(a => a.AssetTag)
            .ToListAsync();

        ViewBag.Assets = new SelectList(
            assets,
            "AssetId",
            "AssetTag");
    }
}