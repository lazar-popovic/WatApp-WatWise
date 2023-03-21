using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class ForgottenPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid email adress!")]
        public required string Email { get; set; } = string.Empty;
    }
}
