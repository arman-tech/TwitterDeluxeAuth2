namespace TwitterThrice.common {
    public static class StringExtensions {
        public static string TruncateMessage(this string message, int maxLength) {

            // sanity check
            if (string.IsNullOrEmpty(message)) {
                return message;
            }

            return message.Length <= maxLength ? message : message.Substring(0, maxLength);
        }

    }

}
