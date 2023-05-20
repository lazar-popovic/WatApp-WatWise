using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class ForgottenPasswordViewModel
    {
        public required string Email { get; set; } = string.Empty;
    }
}
