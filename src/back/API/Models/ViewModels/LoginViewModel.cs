using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class LoginViewModel
    {
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty; //plain text pass for login and register purpose
    }
}
