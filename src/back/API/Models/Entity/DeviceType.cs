using System.ComponentModel.DataAnnotations;

namespace API.Models.Entity;

public class DeviceType
{
    [Key] 
    public int Id { get; set; }
    public string? Type { get; set; } = string.Empty;
    public int? Category { get; set; }
    public List<Device> Devices { get; set; }
}