using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class RegisterEmployeeViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid email adress")]
        public required string Email { get; set; } = string.Empty;
        public required string Firstname { get; set;} = string.Empty;
        public required string Lastname { get; set;} = string.Empty;
    }
}
