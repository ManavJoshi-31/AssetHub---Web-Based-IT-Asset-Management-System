namespace AssetHub.Models;

public class AssetStatusHistory
{
    public int AssetStatusHistoryId { get; set; }

    public int AssetId { get; set; }

    public string OldStatus { get; set; } = string.Empty;

    public string NewStatus { get; set; } = string.Empty;

    public string? ChangedByUserId { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Reason { get; set; }

    public string? Notes { get; set; }
}