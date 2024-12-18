using System.Security.Cryptography;

namespace api.Helpers
{
    public static class PasswordHasher
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            var hashAlgorithm = HashAlgorithmName.SHA256;
            Rfc2898DeriveBytes key = new(password, salt, Iterations, hashAlgorithm);
            var hash = key.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            var hashAlgorithm = HashAlgorithmName.SHA256;
            Rfc2898DeriveBytes key = new(password, salt, Iterations, hashAlgorithm);
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
