using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class ResendVerifyEmailViewModel
    {
        //[EmailAddress(ErrorMessage = "Invalid email adress!")]
        public required string Email { get; set; } = string.Empty;
    }
}
