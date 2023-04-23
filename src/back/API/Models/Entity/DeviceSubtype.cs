using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity
{
    public class DeviceSubtype
    {
        [Key]
        public int Id { get; set; }
        public string? SubtypeName { get; set; }
        public int? DeviceTypeId { get; set; }
        [ForeignKey("DeviceTypeId")]
        public DeviceType? DeviceType { get; set; }
        public List<Device>? Devices { get; set; }
    }
}
