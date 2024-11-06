namespace elite_shop.Helpers;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class HashHelper
{
    // Hash the password with a salt, returning byte[] for both hash and salt
    public static byte[] Hash(string password, out byte[] salt)
    {
        // Generate a 128-bit salt
        salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password using PBKDF2 with HMACSHA256
        return KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32); // 256-bit hash
    }

    // Verify the password against the stored hash and salt
    public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        // Hash the password using the stored salt
        byte[] hashOfInput = KeyDerivation.Pbkdf2(
            password: password,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32); // 256-bit hash

        // Compare the hash of the input password to the stored hash
        return hashOfInput.SequenceEqual(storedHash);
    }
}
