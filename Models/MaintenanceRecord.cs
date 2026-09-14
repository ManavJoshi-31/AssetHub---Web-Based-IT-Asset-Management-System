using AssetHub.Models.Enums;

namespace AssetHub.Models;

public class MaintenanceRecord
{
    public int MaintenanceRecordId { get; set; }

    public int AssetId { get; set; }

    public MaintenanceType MaintenanceType { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Open;

    public string? Vendor { get; set; }

    public decimal? Cost { get; set; }

    public string? TechnicianName { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}