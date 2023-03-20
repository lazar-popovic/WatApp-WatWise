namespace API.Models.ViewModels
{
    public class LoginResponseViewModel
    {
        public required string Token { get;set; }
        public required string RefreshToken { get;set; }
    }
}
