namespace API.Models.ViewModels
{
    public class UpdateUserPasswordViewModel
    {
        public string? OldPassword { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
        public string? ConfirmedPassword { get; set; } = string.Empty;
    }
}
