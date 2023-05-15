using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entity
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime? PurchaseDate { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")] 
        public User? User { get; set; }
        public bool? ActivityStatus { get; set; }
        public List<DeviceEnergyUsage>? DeviceEnergyUsages { get; set; }
        public List<DeviceJob>? DeviceJobs { get; set; }
        
        public int? DeviceTypeId { get; set; }
        [ForeignKey("DeviceTypeId")] 
        public DeviceType? DeviceType { get; set; }

        public bool DataShare { get; set; } = false;
        public bool DsoControl { get; set; } = false;
        public double? Capacity { get; set; }
        public int? DeviceSubtypeId { get; set; }
        [ForeignKey("DeviceSubtypeId")]
        public DeviceSubtype? DeviceSubtype { get; set; }
    }
}
