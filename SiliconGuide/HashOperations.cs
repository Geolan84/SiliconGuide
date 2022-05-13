using System;
using System.Security.Cryptography;
using System.Text;

namespace SiliconGuide
{
    public static class HashOperations
    {
        private static string HashSHA1(string source)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }
        private static string HashMD5(string source)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }
        public static string Encode(string body, string salt)
        {
            return HashSHA1(HashMD5(body) + HashMD5(salt));
        }
    }
}
