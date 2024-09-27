using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using elite_shop.Data;
using elite_shop.Helpers;
using elite_shop.Models.Domains;
using elite_shop.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly EncryptionHelper _encryptionHelper;

    public AuthApiController(ApplicationDbContext context, JwtHelper jwtHelper, EncryptionHelper encryptionHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _encryptionHelper = encryptionHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Encrypt the email before saving
        var encryptedEmail = _encryptionHelper.Encrypt(userDto.Email);

        // Hash the password and generate a salt
        string salt;
        string hashedPassword = HashHelper.HashPassword(userDto.Password, out salt);

        // Create a new user with encrypted email, hashed password, and salt
        var user = new User
        {
            Email = encryptedEmail,
            Password = hashedPassword,
            SaltKey = salt, // Store the salt
            IsActive = true,
            IsDeleted = false,
            LastLoginAt = DateTime.UtcNow
        };

        // Save the user to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);
        return Ok(new { Token = token });
    }
}
