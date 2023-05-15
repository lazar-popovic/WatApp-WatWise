namespace API.Models.ViewModels
{
    public class DeviceNameDsoControlAndDataShareUpdateViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public bool? DataShare { get; set; }
        public bool? DsoControl { get; set; }
    }
}
