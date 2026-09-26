using AssetHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<AssetCategory> AssetCategories { get; set; }

    public DbSet<Asset> Assets { get; set; }

    public DbSet<AssetAssignment> AssetAssignments { get; set; }

    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

    public DbSet<Warranty> Warranties { get; set; }

    public DbSet<AssetStatusHistory> AssetStatusHistories { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Department → Employee
        modelBuilder.Entity<Employee>()
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Department → Manager Employee
        modelBuilder.Entity<Department>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(d => d.ManagerEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // AssetCategory → Asset
        modelBuilder.Entity<Asset>()
            .HasOne<AssetCategory>()
            .WithMany()
            .HasForeignKey(a => a.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Asset → AssetAssignment
        modelBuilder.Entity<AssetAssignment>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(a => a.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee → AssetAssignment
        modelBuilder.Entity<AssetAssignment>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Asset → MaintenanceRecord
        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(m => m.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        // Asset → Warranty (one-to-one)
        modelBuilder.Entity<Warranty>()
            .HasOne<Asset>()
            .WithOne()
            .HasForeignKey<Warranty>(w => w.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Asset → AssetStatusHistory
        modelBuilder.Entity<AssetStatusHistory>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(h => h.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser → Employee
        modelBuilder.Entity<ApplicationUser>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraints
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.DepartmentCode)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.EmployeeCode)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();

        modelBuilder.Entity<AssetCategory>()
            .HasIndex(c => c.CategoryCode)
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.AssetTag)
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.SerialNumber)
            .IsUnique();

        // Decimal precision
        modelBuilder.Entity<Asset>()
            .Property(a => a.PurchaseCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<MaintenanceRecord>()
            .Property(m => m.Cost)
            .HasPrecision(18, 2);
    }
}