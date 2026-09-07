using DevSu.Bank.Customers.Application.Services.Infrastructure;
using System.Security.Cryptography;

namespace DevSu.Bank.Customers.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;
        private const char Separator = '.';

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return string.Join(Separator, Convert.ToBase64String(salt), Convert.ToBase64String(key));
        }
    }
}
