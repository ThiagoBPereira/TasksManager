using System;
using System.Security.Cryptography;

namespace TasksManager.Infra.Cc.Cryptographic
{
    public static class Encrypt
    {
        public static string GenerateSha256Hash(string input, string salt)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            var sha = new SHA1Managed();
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
