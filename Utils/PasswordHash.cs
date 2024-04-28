using System.Security.Cryptography;

namespace dotnet_facebook.Utils
{
    // Hashing passwords using MD5
    public static class PasswordHash
    {
        public static string Create(string password)
        {
            // Use input string to calculate MD5 hash
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            var hashBytes = MD5.HashData(inputBytes);

            return Convert.ToHexString(hashBytes); // .NET 5 +
        }

        public static bool Match(string input, string hash)
        {
            // Hash the input.
            var hashOfInput = Create(input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
