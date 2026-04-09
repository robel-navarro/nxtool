using nxtool.Data;
using nxtool.Helpers;
using nxtool.Models;
using System.Security.Cryptography;
using System.Text;

namespace nxtool.Services
{
    public class TokenService
    {
        private readonly NxToolContext _context;
        private readonly string _passphrase;

        public TokenService(NxToolContext context)
        {

            Console.WriteLine(Environment.GetEnvironmentVariable("SecretKey"));
            _context = context;
            _passphrase = Environment.GetEnvironmentVariable("SecretKey")
                ?? throw new InvalidOperationException("Passphrase not found in environment variables.");
        }

        public void SaveToken(string plainToken)
        {
            var encryptedToken = AesEncryptionHelper.Encrypt(plainToken, _passphrase);

            // Deterministic hash
            var hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(plainToken + _passphrase))
            );

            var token = new TokenRecord
            {
                HashedToken = encryptedToken,
                TokenHash = hash,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            _context.Tokens.Add(token);
            _context.SaveChanges();
        }

        public bool ValidateToken(string plainToken)
        {
            var hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(plainToken + _passphrase))
            );

            return _context.Tokens.Any(t => t.TokenHash == hash && t.ExpiryDate > DateTime.UtcNow);
        }
    }
}
