using AssetHub.Data;
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
}