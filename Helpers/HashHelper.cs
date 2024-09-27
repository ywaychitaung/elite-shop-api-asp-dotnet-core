namespace elite_shop.Helpers;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class HashHelper
{
    // Hash the password with a salt
    public static string HashPassword(string password, out string salt)
    {
        // Generate a 128-bit salt
        byte[] saltBytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        // Convert the salt to a base64 string for storage
        salt = Convert.ToBase64String(saltBytes);

        // Hash the password using PBKDF2 with HMACSHA256
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    // Verify the password against the stored hash and salt
    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        // Convert the stored salt from base64 back to bytes
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        // Hash the password using the stored salt
        string hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Compare the hash of the input password to the stored hash
        return hashOfInput == storedHash;
    }
}
