using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TwitterThrice.common {
    public static class StringExtensions {
        public static string TruncateMessage(this string message, int maxLength) {

            // sanity check
            if (string.IsNullOrEmpty(message)) {
                return message;
            }

            return message.Length <= maxLength ? message : message.Substring(0, maxLength);
        }

        public static bool ContainsXss(this string text) {
            var result = Regex.Match(text, Constants.javascriptRegex);
            return result.Success;
        }

        public static bool ValidEmail(this string email) {
            var result = Regex.Match(email, Constants.emailRegex);
            return result.Success;
        }


        /// <summary>
        /// This is a simple encryption/decryption algorithm that uses AES encryption.  This should not be used in production!
        /// </summary>
        private static byte[] key = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static byte[] iv = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static string Crypt(this string text) {
            using (SymmetricAlgorithm algorithm = Aes.Create()) {
                ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
                byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Convert.ToBase64String(outputBuffer);
            }
        }

        public static string Decrypt(this string text) {
            using (SymmetricAlgorithm algorithm = Aes.Create()) {
                ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
                byte[] inputbuffer = Convert.FromBase64String(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Encoding.Unicode.GetString(outputBuffer);
            }
        }
    }
}
