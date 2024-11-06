using elite_shop.Helpers;

namespace elite_shop.Repositories.Implementations;

using Data;
using Models.Domains;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionHelper _encryptionHelper;

    public UserRepository(ApplicationDbContext context, EncryptionHelper encryptionHelper)
    {
        _context = context;
        _encryptionHelper = encryptionHelper;
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
    public async Task<User?> GetUserByEmailAsync(byte[] encryptedEmail)
    {
        // Query for the user with the encrypted email
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == encryptedEmail);
    }
}
