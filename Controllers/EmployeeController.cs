using AssetHub.Data;
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
}