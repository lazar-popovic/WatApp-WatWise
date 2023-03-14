namespace API.Models.Dto
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmedNewPassword { get; set; } = string.Empty;    
    }
}
