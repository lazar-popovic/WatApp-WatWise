namespace API.Models.Entity;

public class DeviceJob
{
    public int? Id { get; set; }
    public int? DeviceId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? StartJobId { get; set; }
    public int? EndJobId { get; set; }
    public bool? Turn { get; set; }
    public bool? Repeat { get; set; }
}