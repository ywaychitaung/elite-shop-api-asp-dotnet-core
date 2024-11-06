namespace elite_shop.Helpers;

using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

public class EncryptionHelper
{
    private readonly byte[] _encryptionKey;
    private readonly byte[] _iv;

    public EncryptionHelper(IConfiguration configuration)
    {
        // Fetch the encryption key from configuration and decode it from Base64
        var base64Key = configuration["AesSettings:EncryptionKey"];
        _encryptionKey = Convert.FromBase64String(base64Key);
        
        // Check that the decoded key is 32 bytes long for AES-256
        if (_encryptionKey.Length != 32)
            throw new ArgumentException("Encryption key must be 32 bytes long for AES-256.");

        // Fetch the IV from configuration and decode it from Base64
        var base64IV = configuration["AesSettings:IV"];
        _iv = Convert.FromBase64String(base64IV);

        // Check that the IV is 16 bytes long for AES
        if (_iv.Length != 16)
            throw new ArgumentException("IV must be 16 bytes long for AES.");
    }

    public byte[] Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _encryptionKey;
            aes.IV = _iv;  // Use fixed IV from configuration
            
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                // Write encrypted data to the memory stream
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                // Return the encrypted data as a base64-encoded string
                return ms.ToArray();
            }
        }
    }

    public string Decrypt(byte[] cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _encryptionKey;
            aes.IV = _iv;  // Use the same fixed IV for decryption

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(cipherText))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                // Return the decrypted plaintext
                return sr.ReadToEnd();
            }
        }
    }
}
