using System;
using System.Security.Cryptography;
using System.Text;

namespace NongSanShop.Util
{
    public class HMac256Helper
    {
        // password format base64(saltyPasswordHased)
        public static string HashPassword(string rawPassword, string salt)
        {
            var passwordBytes = Encoding.Unicode.GetBytes(rawPassword);
            var saltBytes = Encoding.Unicode.GetBytes(salt);
            var saltyPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, saltyPasswordBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltyPasswordBytes, saltBytes.Length, passwordBytes.Length);

            return Convert.ToBase64String(new HMACSHA256(saltBytes).ComputeHash(saltyPasswordBytes));
        }

        public static bool ProcessAuthentication(string rawPassword, string salt, string hashedPassword)
        {
            return hashedPassword.Equals(HashPassword(rawPassword, salt));
        }

    }
}