using salacerta_auth_service.DTOs;
using salacerta_auth_service.Models;
using salacerta_auth_service.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace salacerta_auth_service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task RegisterAsync(RegisterDTO dto)
    {
        // 1. Verifica se o usuário já existe
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("Este e-mail já está em uso.");

        // 2. Cria o Hash da senha usando BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. Monta a entidade e salva no banco
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.CreateAsync(user);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        // 1. Busca o usuário no banco
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("Credenciais inválidas.");

        // 2. Verifica se a senha bate com o Hash
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Credenciais inválidas.");

        // 3. Gera o Token JWT
        var token = GenerateJwtToken(user);

        return new AuthResponseDTO
        {
            Token = token,
            Email = user.Email
        };
    }

    private string GenerateJwtToken(User user)
    {
        // A chave secreta deve vir do seu arquivo .env
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
        if (string.IsNullOrEmpty(secretKey))
            throw new Exception("JWT_SECRET não configurado no servidor.");

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}