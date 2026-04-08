using System.Security.Cryptography;
using System.Text;

namespace nxtool.Helpers
{
    public static class AesEncryptionHelper
    {
        public static string Encrypt(string plainText, string passphrase)
        {
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(passphrase)); // derive 256-bit key
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // store IV + cipher together
            var combined = aes.IV.Concat(cipherBytes).ToArray();
            return Convert.ToBase64String(combined);
        }

        public static string Decrypt(string cipherText, string passphrase)
        {
            var combined = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(passphrase));

            var iv = combined.Take(aes.BlockSize / 8).ToArray();
            var cipherBytes = combined.Skip(aes.BlockSize / 8).ToArray();

            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
