namespace TwitterThrice.common {
    public static class Constants {

        public const int MaxTweetMessageLength = 140;
        public const int DefaultTweetCount = 10;
        public const string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string javascriptRegex = @"<script.*?>.*?</script>|javascript:";
    }
}
