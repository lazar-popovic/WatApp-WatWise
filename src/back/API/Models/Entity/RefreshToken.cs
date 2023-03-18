using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? user { get; set; }
    }
}
