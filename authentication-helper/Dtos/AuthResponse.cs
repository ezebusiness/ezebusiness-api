namespace authentication_helper
{
    public class AuthResponse<TUser> where TUser : class
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public TUser User { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse() {
        
        }

        public AuthResponse(bool success, string accessToken, string refreshToken, TUser user)
        {
            Success = success;
            Token = accessToken;
            RefreshToken = refreshToken;
            User = user;
        }
    }
}
