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
public class AssetAssignmentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AssetAssignmentService _assignmentService;

    public AssetAssignmentController(
        ApplicationDbContext context,
        AssetAssignmentService assignmentService)
    {
        _context = context;
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadDropdownsAsync();

        return View(new AssetAssignmentViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssetAssignmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return View(model);
        }

        var assignedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _assignmentService.AssignAssetAsync(
            model.AssetId,
            model.EmployeeId,
            assignedByUserId,
            model.AssignmentNotes);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);

            await LoadDropdownsAsync();
            return View(model);
        }

        TempData["SuccessMessage"] = "Asset assigned successfully.";

        return RedirectToAction(nameof(Create));
    }

    private async Task LoadDropdownsAsync()
    {
        var employees = await _context.Employees
            .Where(e => e.IsActive)
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();

        var assets = await _context.Assets
            .Where(a => a.IsActive &&
                        a.AssetStatus == Models.Enums.AssetStatus.Available)
            .OrderBy(a => a.AssetTag)
            .ToListAsync();

        ViewBag.Employees = new SelectList(
            employees,
            "EmployeeId",
            "EmployeeCode");

        ViewBag.Assets = new SelectList(
            assets,
            "AssetId",
            "AssetTag");
    }
}