using Microsoft.AspNetCore.Http;

namespace authentication_helper
{
    public interface IUserHelper
    {
        string GetCurrentUserIdentity(HttpRequest req);
    }

    public class UserHelper : IUserHelper
    {
        private readonly ITokenHelper _tokenHelper;

        public UserHelper(ITokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        public string GetCurrentUserIdentity(HttpRequest req)
        {
            string authorizationHeader = req.Headers["Authorization"];

            if (authorizationHeader.StartsWith("Bearer"))
                authorizationHeader = authorizationHeader.Substring(7);

            var validatedLogin = _tokenHelper.ValidateJwtToken(authorizationHeader);

            if (validatedLogin != null)
            {
                foreach (var claim in validatedLogin.Claims)
                {
                    if (claim.Type == "unique_name")
                        return claim.Value;
                }
            }

            return string.Empty;
        }
    }
}
