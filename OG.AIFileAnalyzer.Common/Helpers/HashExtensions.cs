using System.Security.Cryptography;
using System.Text;

namespace OG.AIFileAnalyzer.Common.Helpers
{
    /// <summary>
    /// Provides extension methods for hashing operations.
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        /// Computes the SHA256 hash of the byte array.
        /// </summary>
        /// <param name="byteArray">The byte array to compute the hash from.</param>
        /// <returns>The SHA256 hash value as a hexadecimal string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the byte array is null.</exception>
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
