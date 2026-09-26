using AssetHub.Data;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class AssetController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssetController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Asset
    public async Task<IActionResult> Index()
    {
        var assets = await (
            from asset in _context.Assets.AsNoTracking()
            join category in _context.AssetCategories.AsNoTracking()
                on asset.AssetCategoryId equals category.AssetCategoryId
            orderby asset.AssetName
            select new AssetListViewModel
            {
                AssetId = asset.AssetId,
                AssetTag = asset.AssetTag,
                AssetName = asset.AssetName,
                SerialNumber = asset.SerialNumber,
                CategoryName = category.CategoryName,
                Brand = asset.Brand,
                Model = asset.Model,
                AssetStatus = asset.AssetStatus,
                Condition = asset.Condition,
                Location = asset.Location,
                IsActive = asset.IsActive
            }
        ).ToListAsync();

        return View(assets);
    }
}