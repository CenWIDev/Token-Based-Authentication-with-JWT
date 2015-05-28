using System.Security.Cryptography;
using System.Text;

namespace TokenAuth.Api.Factories
{
    public static class HashFactory
    {
        private static byte[] GetHash(string str)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
        }

        public static string GetHashString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(str))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}