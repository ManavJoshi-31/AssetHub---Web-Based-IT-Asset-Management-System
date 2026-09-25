using AssetHub.Data;
using AssetHub.Models;
using AssetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await (
            from employee in _context.Employees
            join department in _context.Departments
                on employee.DepartmentId equals department.DepartmentId
            orderby employee.LastName, employee.FirstName
            select new EmployeeListViewModel
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                DepartmentName = department.DepartmentName,
                DateOfJoining = employee.DateOfJoining,
                EmploymentStatus = employee.EmploymentStatus,
                IsActive = employee.IsActive
            }
        ).ToListAsync();

        return View(employees);
    }


    // GET: /Employee/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await _context.Departments
            .Where(d => d.IsActive)
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();

        return View();
    }

    // POST: /Employee/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        bool employeeCodeExists = await _context.Employees
            .AnyAsync(e => e.EmployeeCode == model.EmployeeCode);

        if (employeeCodeExists)
        {
            ModelState.AddModelError(
                nameof(model.EmployeeCode),
                "Employee code already exists.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        bool emailExists = await _context.Employees
            .AnyAsync(e => e.Email == model.Email);

        if (emailExists)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Employee email already exists.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        bool departmentExists = await _context.Departments
            .AnyAsync(d =>
                d.DepartmentId == model.DepartmentId &&
                d.IsActive);

        if (!departmentExists)
        {
            ModelState.AddModelError(
                nameof(model.DepartmentId),
                "Please select a valid active department.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        var employee = new Employee
        {
            EmployeeCode = model.EmployeeCode,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DepartmentId = model.DepartmentId,
            JobTitle = model.JobTitle,
            DateOfJoining = model.DateOfJoining,
            DateOfLeaving = model.DateOfLeaving,
            EmploymentStatus = model.EmploymentStatus,
            Address = model.Address,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: /Employee/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null)
        {
            return NotFound();
        }

        ViewBag.Departments = await _context.Departments
            .Where(d => d.IsActive)
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();

        var model = new EmployeeViewModel
        {
            EmployeeId = employee.EmployeeId,
            EmployeeCode = employee.EmployeeCode,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            DepartmentId = employee.DepartmentId,
            JobTitle = employee.JobTitle,
            DateOfJoining = employee.DateOfJoining,
            DateOfLeaving = employee.DateOfLeaving,
            EmploymentStatus = employee.EmploymentStatus,
            Address = employee.Address,
            IsActive = employee.IsActive
        };

        return View(model);
    }

    // POST: /Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
    {
        if (id != model.EmployeeId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null)
        {
            return NotFound();
        }

        bool employeeCodeExists = await _context.Employees
            .AnyAsync(e =>
                e.EmployeeCode == model.EmployeeCode &&
                e.EmployeeId != id);

        if (employeeCodeExists)
        {
            ModelState.AddModelError(
                nameof(model.EmployeeCode),
                "Employee code already exists.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        bool emailExists = await _context.Employees
            .AnyAsync(e =>
                e.Email == model.Email &&
                e.EmployeeId != id);

        if (emailExists)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Employee email already exists.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        bool departmentExists = await _context.Departments
            .AnyAsync(d =>
                d.DepartmentId == model.DepartmentId &&
                d.IsActive);

        if (!departmentExists)
        {
            ModelState.AddModelError(
                nameof(model.DepartmentId),
                "Please select a valid active department.");

            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(model);
        }

        employee.EmployeeCode = model.EmployeeCode;
        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.Email = model.Email;
        employee.PhoneNumber = model.PhoneNumber;
        employee.DepartmentId = model.DepartmentId;
        employee.JobTitle = model.JobTitle;
        employee.DateOfJoining = model.DateOfJoining;
        employee.DateOfLeaving = model.DateOfLeaving;
        employee.EmploymentStatus = model.EmploymentStatus;
        employee.Address = model.Address;
        employee.IsActive = model.IsActive;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}