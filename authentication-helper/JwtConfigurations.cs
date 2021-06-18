namespace authentication_helper
{
    public static class JwtConfigurations
    {
        public static readonly string SECRET_KEY = "YOUR_SECRETY_KEY_JUST_A_LONG_STRING";
        public static readonly int TOKEN_EXPIRY_IN_MINUTES = 15;
        public static readonly int REFRESH_EXPIRY_IN_DAYS = 100;
    }
}
