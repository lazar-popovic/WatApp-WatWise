using API.Models.Entity;

namespace API.Models;

public class UserWithCurrentProdAndCons
{
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public string? Location { get; set; }
    public int? LocationId { get; set; }
    public string? Email { get; set; }
    public bool? Verified { get; set; }
    public double? CurrentProduction { get; set; }
    public double? CurrentConsumption { get; set; }
}