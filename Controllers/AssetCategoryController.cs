using AssetHub.Data;
using AssetHub.Models;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class AssetCategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssetCategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /AssetCategory
    public async Task<IActionResult> Index()
    {
        var categories = await _context.AssetCategories
            .OrderBy(c => c.CategoryName)
            .Select(c => new AssetCategoryViewModel
            {
                AssetCategoryId = c.AssetCategoryId,
                CategoryName = c.CategoryName,
                CategoryCode = c.CategoryCode,
                Description = c.Description,
                IsActive = c.IsActive
            })
            .ToListAsync();

        return View(categories);
    }



    // GET: /AssetCategory/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /AssetCategory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssetCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool categoryCodeExists = await _context.AssetCategories
            .AnyAsync(c => c.CategoryCode == model.CategoryCode);

        if (categoryCodeExists)
        {
            ModelState.AddModelError(
                nameof(model.CategoryCode),
                "Category code already exists.");

            return View(model);
        }

        var category = new AssetCategory
        {
            CategoryName = model.CategoryName,
            CategoryCode = model.CategoryCode,
            Description = model.Description,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.AssetCategories.Add(category);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}