using System.Security.Cryptography;
using System.Text;

namespace AqoTesting.Domain.Utils
{
    public static class Sha256
    {
        public static byte[] Compute(string password)
        {
            byte[] bytedPassword = Encoding.UTF8.GetBytes(password);
            byte[] hash;
            using(SHA256 sha256 = SHA256.Create()) { hash = sha256.ComputeHash(bytedPassword); }

            return hash;
        }
    }
}
