using salacerta_auth_service.DTOs;

namespace salacerta_auth_service.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDTO dto);

    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
}