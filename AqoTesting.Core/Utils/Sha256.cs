using System.Security.Cryptography;
using System.Text;

namespace AqoTesting.Core.Utils
{
    public static class Sha256
    {
        public static byte[] Compute(string data)
        {
            byte[] bytedData = Encoding.UTF8.GetBytes(data);

            byte[] hash;
            using(SHA256 sha256 = SHA256.Create())
                hash = sha256.ComputeHash(bytedData);

            return hash;
        }
    }
}