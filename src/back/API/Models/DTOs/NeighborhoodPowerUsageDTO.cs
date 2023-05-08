namespace API.Models.DTOs;

public class NeighborhoodPowerUsageDTO
{
    public string Neighborhood { get; set; } = string.Empty;
    public double? PowerUsage { get; set; }
    public double? PredictedPowerUsage { get; set; }
}