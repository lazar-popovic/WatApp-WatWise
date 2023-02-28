using System.ComponentModel.DataAnnotations;

namespace API.Model;

public class Device
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public double Price { get; set; }
}