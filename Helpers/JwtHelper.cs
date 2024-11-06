namespace elite_shop.Helpers;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using elite_shop.Models.Domains;

public class JwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly EncryptionHelper _encryptionHelper;

    public JwtHelper(IConfiguration configuration, EncryptionHelper encryptionHelper)
    {
        _configuration = configuration;
        _encryptionHelper = encryptionHelper;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        // Decrypt the user's email to a string
        var decryptedEmail = _encryptionHelper.Decrypt(user.Email);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, decryptedEmail), // Use decrypted email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.RoleId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
