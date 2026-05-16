namespace salacerta_auth_service.DTOs;

public class LoginDTO // <-- O nome exato que o Controller pede
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}