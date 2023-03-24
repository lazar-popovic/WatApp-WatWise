﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity
{
    public class DeviceEnergyUsage
    {
        [Key]
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? Value { get; set; }
    }
}