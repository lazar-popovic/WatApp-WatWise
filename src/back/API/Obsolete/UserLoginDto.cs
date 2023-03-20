namespace API.Obsolete;

public class UserLoginDto
{
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty; //plain text pass for login and register purpose
}