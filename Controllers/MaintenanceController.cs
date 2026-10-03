using AssetHub.Data;
using AssetHub.Services;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class MaintenanceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly MaintenanceService _maintenanceService;

    public MaintenanceController(
        ApplicationDbContext context,
        MaintenanceService maintenanceService)
    {
        _context = context;
        _maintenanceService = maintenanceService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var maintenanceRecords = await _context.MaintenanceRecords
            .OrderByDescending(m => m.StartDate)
            .ToListAsync();

        return View(maintenanceRecords);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(int id)
    {
        var result = await _maintenanceService.StartMaintenanceAsync(id);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] =
            "Maintenance has been started successfully.";

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var result =
            await _maintenanceService.CompleteMaintenanceAsync(id);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] =
            "Maintenance has been completed successfully.";

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadAssetsAsync();

        var model = new MaintenanceViewModel
        {
            StartDate = DateTime.Today
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MaintenanceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadAssetsAsync();
            return View(model);
        }

        var result = await _maintenanceService.CreateMaintenanceAsync(
            model.AssetId,
            model.MaintenanceType,
            model.Description,
            model.StartDate,
            model.Vendor,
            model.Cost,
            model.TechnicianName,
            model.Notes);

        if (!result.Success)
        {
            ModelState.AddModelError(
                string.Empty,
                result.ErrorMessage!);

            await LoadAssetsAsync();
            return View(model);
        }

        TempData["SuccessMessage"] =
            "Maintenance record created successfully.";

        return RedirectToAction(nameof(Create));
    }

    private async Task LoadAssetsAsync()
    {
        var assets = await _context.Assets
            .Where(a => a.IsActive &&
                        a.AssetStatus == Models.Enums.AssetStatus.Available)
            .OrderBy(a => a.AssetTag)
            .ToListAsync();

        ViewBag.Assets = new SelectList(
            assets,
            "AssetId",
            "AssetTag");
    }
}