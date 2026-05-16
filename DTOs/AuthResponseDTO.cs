namespace salacerta_auth_service.DTOs;

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}