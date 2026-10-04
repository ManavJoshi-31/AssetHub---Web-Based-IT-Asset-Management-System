using AssetHub.Data;
using AssetHub.Models;
using AssetHub.Services;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class WarrantyController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly WarrantyService _warrantyService;

    public WarrantyController(
        ApplicationDbContext context,
        WarrantyService warrantyService)
    {
        _context = context;
        _warrantyService = warrantyService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadAssetsAsync();

        return View(new WarrantyViewModel
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WarrantyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadAssetsAsync();
            return View(model);
        }

        var warranty = new Warranty
        {
            AssetId = model.AssetId,
            WarrantyType = model.WarrantyType,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Provider = model.Provider,
            WarrantyNumber = model.WarrantyNumber,
            TermsAndConditions = model.TermsAndConditions,
            Notes = model.Notes
        };

        var result = await _warrantyService.CreateWarrantyAsync(warranty);

        if (!result.Success)
        {
            ModelState.AddModelError(
                string.Empty,
                result.ErrorMessage!);

            await LoadAssetsAsync();
            return View(model);
        }

        TempData["SuccessMessage"] =
            "Warranty created successfully.";

        return RedirectToAction(nameof(Create));
    }

    private async Task LoadAssetsAsync()
    {
        var assets = await _context.Assets
            .Where(a => a.IsActive)
            .OrderBy(a => a.AssetTag)
            .ToListAsync();

        ViewBag.Assets = new SelectList(
            assets,
            "AssetId",
            "AssetTag");
    }
}