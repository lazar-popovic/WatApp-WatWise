namespace API.Models.DTOs;

public class LocationWithPowerUsageDTO
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public int? AddressNumber { get; set; }
    public string? City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Neighborhood { get; set; }
    public double? TotalPowerUsage { get; set; } 
}