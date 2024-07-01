namespace TwitterThrice.common {
    public static class Constants {

        public static int MaxTweetMessageLength = 140;
        public static int DefaultTweetCount = 10;
        public static string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public static string javascriptRegex = @"<script.*?>.*?</script>|javascript:";
    }
}
