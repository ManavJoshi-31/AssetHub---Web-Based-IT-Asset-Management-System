using AssetHub.Data;
using AssetHub.Models;
using AssetHub.Models.Enums;
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

    // GET: /Asset/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();

        return View();
    }

    // POST: /Asset/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssetViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(viewModel);
        }

        var assetTagExists = await _context.Assets
            .AnyAsync(a => a.AssetTag == viewModel.AssetTag);

        if (assetTagExists)
        {
            ModelState.AddModelError(
                nameof(viewModel.AssetTag),
                "An asset with this Asset Tag already exists.");

            await LoadCategoriesAsync();
            return View(viewModel);
        }

        if (!string.IsNullOrWhiteSpace(viewModel.SerialNumber))
        {
            var serialNumberExists = await _context.Assets
                .AnyAsync(a => a.SerialNumber == viewModel.SerialNumber);

            if (serialNumberExists)
            {
                ModelState.AddModelError(
                    nameof(viewModel.SerialNumber),
                    "An asset with this Serial Number already exists.");

                await LoadCategoriesAsync();
                return View(viewModel);
            }
        }

        var categoryExists = await _context.AssetCategories
            .AnyAsync(c =>
                c.AssetCategoryId == viewModel.AssetCategoryId &&
                c.IsActive);

        if (!categoryExists)
        {
            ModelState.AddModelError(
                nameof(viewModel.AssetCategoryId),
                "Please select a valid active asset category.");

            await LoadCategoriesAsync();
            return View(viewModel);
        }

        var asset = new Asset
        {
            AssetTag = viewModel.AssetTag.Trim(),
            AssetName = viewModel.AssetName.Trim(),

            SerialNumber = string.IsNullOrWhiteSpace(viewModel.SerialNumber)
                ? null
                : viewModel.SerialNumber.Trim(),

            AssetCategoryId = viewModel.AssetCategoryId,

            Brand = string.IsNullOrWhiteSpace(viewModel.Brand)
                ? null
                : viewModel.Brand.Trim(),

            Model = string.IsNullOrWhiteSpace(viewModel.Model)
                ? null
                : viewModel.Model.Trim(),

            Description = string.IsNullOrWhiteSpace(viewModel.Description)
                ? null
                : viewModel.Description.Trim(),

            PurchaseDate = viewModel.PurchaseDate,
            PurchaseCost = viewModel.PurchaseCost,

            Vendor = string.IsNullOrWhiteSpace(viewModel.Vendor)
                ? null
                : viewModel.Vendor.Trim(),

            InvoiceNumber = string.IsNullOrWhiteSpace(viewModel.InvoiceNumber)
                ? null
                : viewModel.InvoiceNumber.Trim(),

            Location = string.IsNullOrWhiteSpace(viewModel.Location)
                ? null
                : viewModel.Location.Trim(),

            AssetStatus = AssetStatus.Available,
            Condition = viewModel.Condition,
            WarrantyExpiryDate = viewModel.WarrantyExpiryDate,

            Notes = string.IsNullOrWhiteSpace(viewModel.Notes)
                ? null
                : viewModel.Notes.Trim(),

            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync()
    {
        ViewBag.Categories = await _context.AssetCategories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }



    // GET: /Asset/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var asset = await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AssetId == id);

        if (asset == null)
        {
            return NotFound();
        }

        var viewModel = new AssetViewModel
        {
            AssetId = asset.AssetId,
            AssetTag = asset.AssetTag,
            AssetName = asset.AssetName,
            SerialNumber = asset.SerialNumber,
            AssetCategoryId = asset.AssetCategoryId,
            Brand = asset.Brand,
            Model = asset.Model,
            Description = asset.Description,
            PurchaseDate = asset.PurchaseDate,
            PurchaseCost = asset.PurchaseCost,
            Vendor = asset.Vendor,
            InvoiceNumber = asset.InvoiceNumber,
            Location = asset.Location,
            Condition = asset.Condition,
            WarrantyExpiryDate = asset.WarrantyExpiryDate,
            Notes = asset.Notes
        };

        await LoadCategoriesAsync();

        return View(viewModel);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AssetViewModel viewModel)
    {
        if (id != viewModel.AssetId)
        {
            return BadRequest();
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetId == id);

        if (asset == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(viewModel);
        }

        var assetTagExists = await _context.Assets
            .AnyAsync(a =>
                a.AssetTag == viewModel.AssetTag &&
                a.AssetId != id);

        if (assetTagExists)
        {
            ModelState.AddModelError(
                nameof(viewModel.AssetTag),
                "An asset with this Asset Tag already exists.");

            await LoadCategoriesAsync();
            return View(viewModel);
        }

        if (!string.IsNullOrWhiteSpace(viewModel.SerialNumber))
        {
            var serialNumberExists = await _context.Assets
                .AnyAsync(a =>
                    a.SerialNumber == viewModel.SerialNumber &&
                    a.AssetId != id);

            if (serialNumberExists)
            {
                ModelState.AddModelError(
                    nameof(viewModel.SerialNumber),
                    "An asset with this Serial Number already exists.");

                await LoadCategoriesAsync();
                return View(viewModel);
            }
        }

        var categoryExists = await _context.AssetCategories
            .AnyAsync(c =>
                c.AssetCategoryId == viewModel.AssetCategoryId &&
                c.IsActive);

        if (!categoryExists)
        {
            ModelState.AddModelError(
                nameof(viewModel.AssetCategoryId),
                "Please select a valid active asset category.");

            await LoadCategoriesAsync();
            return View(viewModel);
        }

        asset.AssetTag = viewModel.AssetTag.Trim();
        asset.AssetName = viewModel.AssetName.Trim();

        asset.SerialNumber = string.IsNullOrWhiteSpace(viewModel.SerialNumber)
            ? null
            : viewModel.SerialNumber.Trim();

        asset.AssetCategoryId = viewModel.AssetCategoryId;

        asset.Brand = string.IsNullOrWhiteSpace(viewModel.Brand)
            ? null
            : viewModel.Brand.Trim();

        asset.Model = string.IsNullOrWhiteSpace(viewModel.Model)
            ? null
            : viewModel.Model.Trim();

        asset.Description = string.IsNullOrWhiteSpace(viewModel.Description)
            ? null
            : viewModel.Description.Trim();

        asset.PurchaseDate = viewModel.PurchaseDate;
        asset.PurchaseCost = viewModel.PurchaseCost;

        asset.Vendor = string.IsNullOrWhiteSpace(viewModel.Vendor)
            ? null
            : viewModel.Vendor.Trim();

        asset.InvoiceNumber = string.IsNullOrWhiteSpace(viewModel.InvoiceNumber)
            ? null
            : viewModel.InvoiceNumber.Trim();

        asset.Location = string.IsNullOrWhiteSpace(viewModel.Location)
            ? null
            : viewModel.Location.Trim();

        asset.Condition = viewModel.Condition;
        asset.WarrantyExpiryDate = viewModel.WarrantyExpiryDate;

        asset.Notes = string.IsNullOrWhiteSpace(viewModel.Notes)
            ? null
            : viewModel.Notes.Trim();

        asset.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}