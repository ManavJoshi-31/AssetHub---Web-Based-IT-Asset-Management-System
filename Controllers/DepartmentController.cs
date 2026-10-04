using AssetHub.Data;
using AssetHub.Models;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class DepartmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepartmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Department
    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();

        return View(departments);
    }

    // GET: /Department/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Department/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        bool codeExists = await _context.Departments
            .AnyAsync(d => d.DepartmentCode == model.DepartmentCode);

        if (codeExists)
        {
            ModelState.AddModelError(
                nameof(model.DepartmentCode),
                "Department code already exists.");

            return View(model);
        }

        var department = new Department
        {
            DepartmentName = model.DepartmentName,
            DepartmentCode = model.DepartmentCode,
            Description = model.Description,
            ManagerEmployeeId = null,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: /Department/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return NotFound();

        var model = new DepartmentViewModel
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            DepartmentCode = department.DepartmentCode,
            Description = department.Description,
            ManagerEmployeeId = department.ManagerEmployeeId,
            IsActive = department.IsActive
        };

        return View(model);
    }

    // POST: /Department/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        DepartmentViewModel model)
    {
        if (id != model.DepartmentId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        bool codeExists = await _context.Departments
            .AnyAsync(d =>
                d.DepartmentCode == model.DepartmentCode &&
                d.DepartmentId != model.DepartmentId);

        if (codeExists)
        {
            ModelState.AddModelError(
                nameof(model.DepartmentCode),
                "Department code already exists.");

            return View(model);
        }

        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return NotFound();

        department.DepartmentName = model.DepartmentName;
        department.DepartmentCode = model.DepartmentCode;
        department.Description = model.Description;
        department.IsActive = model.IsActive;
        department.UpdatedAt = DateTime.UtcNow;

        // ManagerEmployeeId remains unchanged for now.
        // Employee management will be implemented later.

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // POST: /Department/ToggleStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return NotFound();

        department.IsActive = !department.IsActive;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}