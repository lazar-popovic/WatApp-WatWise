using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class RegisterEmployeeViewModel
    {
        public required string Email { get; set; } = string.Empty;
        public required string Firstname { get; set;} = string.Empty;
        public required string Lastname { get; set;} = string.Empty;
    }
}
