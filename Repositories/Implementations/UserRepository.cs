namespace elite_shop.Repositories.Implementations;

using elite_shop.Data;
using elite_shop.Models.Domains;
using elite_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a user to the database
    public async Task AddUserAsync(User user)
    {
        // Add the user to the Users DbSet
        _context.Users.Add(user);
        
        // Save changes to the database
        await _context.SaveChangesAsync();
    }

    // Get a user by email
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // Use FirstOrDefaultAsync to get the first user with the specified email
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
