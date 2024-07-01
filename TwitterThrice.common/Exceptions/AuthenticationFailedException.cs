namespace TwitterDeluxeAuth2.common.Exceptions {

    [Serializable]
    public class AuthenticationFailedException : Exception {
        public AuthenticationFailedException() : base() { }

        public AuthenticationFailedException(string message) : base(message) { }

        public AuthenticationFailedException(string message, Exception inner) : base(message, inner) { }

    }
}
