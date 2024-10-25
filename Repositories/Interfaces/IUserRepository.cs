namespace elite_shop.Repositories.Interfaces;

using elite_shop.Models.Domains;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}
