using salacerta_auth_service.Models;

namespace salacerta_auth_service.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task CreateAsync(User user);
}