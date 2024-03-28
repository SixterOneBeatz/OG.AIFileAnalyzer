using System.Security.Cryptography;
using System.Text;

namespace OG.AIFileAnalyzer.Common.Helpers
{
    public static class HashExtensions
    {
        public static string ComputeSHA256Hash(this byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException(nameof(byteArray));

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(byteArray);
                StringBuilder builder = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
