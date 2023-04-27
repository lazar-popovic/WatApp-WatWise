namespace API.Models.ViewModels
{
    public class DeviceViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public int? DeviceTypeId { get; set; }
        public int? DeviceSubtypeId { get; set; }
        public int? UserId { get; set; }
        public double Capacity { get; set; }
        public int? Category { get; set; }

    }
}
