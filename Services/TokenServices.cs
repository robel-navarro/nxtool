using nxtool.Data;
using nxtool.Models;
using nxtool.Helpers;

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

            var token = new TokenRecord
            {
                HashedToken = encryptedToken,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            _context.Tokens.Add(token);
            _context.SaveChanges();
        }

        public bool ValidateToken(string plainToken)
        {
            var encryptedToken = AesEncryptionHelper.Encrypt(plainToken, _passphrase);
            return _context.Tokens.Any(t => t.HashedToken == encryptedToken && t.ExpiryDate > DateTime.UtcNow);
        }
    }
}
