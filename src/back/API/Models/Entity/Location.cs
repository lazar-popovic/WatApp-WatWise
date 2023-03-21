using System.ComponentModel.DataAnnotations;

namespace API.Models.Entity;

public class Location
{
    [Key]
    public int Id { get; set; }
    public string? Address { get; set; } = string.Empty;
    public int? AddressNumber { get; set; }
    public string? City { get; set; } = string.Empty;

    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    public List<User>? Users { get; set; }
}