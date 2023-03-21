using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entity
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public int? Category { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")] 
        public User? User { get; set; }
        public bool? ActivityStatus { get; set; }
    }
}
