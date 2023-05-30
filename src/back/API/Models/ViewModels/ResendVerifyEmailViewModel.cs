using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class ResendVerifyEmailViewModel
    {
        public required string Email { get; set; } = string.Empty;
    }
}
