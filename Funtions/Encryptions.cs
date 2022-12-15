using System.Security.Cryptography;

namespace TimeKeeper.Funtions
{
    public class Encryptions
    {
        private static readonly Random random = new();
        public static string HashPassword(string password, string Salt)
        {
            byte[] PasswordWithSaltBytes = System.Text.Encoding.ASCII.GetBytes((password + Salt));

            using HashAlgorithm HasGenerator = SHA256.Create();
            byte[] HashComputed = HasGenerator.ComputeHash(PasswordWithSaltBytes);

            string HashString = Convert.ToBase64String(HashComputed);

            return HashString;
        }
        public static string GeneratorRamdonNumber(int Length)
        {
            byte[] randomNumer = new byte[Length];
            string refreshToken = string.Empty;

            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumer);
                refreshToken = Convert.ToBase64String(randomNumer);
            }

            return refreshToken;
        }
        public string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
    }
}
