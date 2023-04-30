namespace API.Models.ViewModels;

public class DeviceJobViewModel
{
    public int? DeviceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? Turn { get; set; }
    public bool Repeat { get; set; }
}