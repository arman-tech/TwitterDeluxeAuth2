using System.Text.RegularExpressions;

namespace TwitterThrice.common {
    public static class Helper {
        public static bool ContainsXss(string input) {
            const string pattern = @"<script.*?>.*?</script>|javascript:";
            var result = Regex.Match(input, pattern);

            return result.Success;
        }
    }
}
