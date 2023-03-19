namespace API.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = string.Empty;
        public required string OldPassword { get; set; } = string.Empty;
        public required string NewPassword { get; set; } = string.Empty;
        public required string ConfirmedNewPassword { get; set; } = string.Empty;
    }
}
