namespace API.Models.Dto;

public class VerifyAccountDto
{
    public string Token { get; set; } = string.Empty;
    public string Password1 { get; set; } = string.Empty;
    public string Password2 { get; set; } = string.Empty;
}