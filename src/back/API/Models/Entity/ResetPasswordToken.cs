using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity
{
    public class ResetPasswordToken
    {
        [Key]
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }

}
