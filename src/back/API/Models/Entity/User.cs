using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = string.Empty;
    public string? Firstname { get; set; } = string.Empty;
    public string? Lastname { get; set; } = string.Empty;
    public bool? Verified { get; set; }
    public int? RoleId { get; set; }
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }
    public int? LocationId { get; set; }
    [ForeignKey("LocationId")]
    public Location? Location { get; set; }

    public List<Device>? Devices { get; set; }
    public byte[]? ProfileImage { get; set; }

    public User()
    {
        
        var defaultImageData = System.IO.File.ReadAllBytes("putanja/do/podrazumevane/slike.jpg");

       
        if (ProfileImage == null)
        {
            ProfileImage = defaultImageData;
        }
    }
}
