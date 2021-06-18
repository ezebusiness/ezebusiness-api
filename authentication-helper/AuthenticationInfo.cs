using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace authentication_helper
{
    /// <summary>
    ///     Wrapper class for encapsulating claims parsing.
    /// </summary>
    public class AuthenticationInfo
    {
        public bool IsValid { get; }
        public string Username { get; }
        public string Role { get; }

        public AuthenticationInfo(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization"))
            {
                IsValid = false;

                return;
            }

            string authorizationHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                IsValid = false;

                return;
            }

            IDictionary<string, object> claims = null;

            try
            {
                if (authorizationHeader.StartsWith("Bearer"))
                {
                    authorizationHeader = authorizationHeader.Substring(7);
                }

                // Validate the token and decode the claims.
                //claims = new JwtBuilder()
                //    .WithAlgorithm(new HMACSHA256Algorithm())
                //    .WithSecret(JwtConfigurations.SECRET_KEY)
                //    .MustVerifySignature()
                //    .Decode<IDictionary<string, object>>(authorizationHeader);
            }
            catch (Exception exception)
            {
                IsValid = false;

                return;
            }

            // Check if we have user claim.
            if (!claims.ContainsKey("username"))
            {
                IsValid = false;

                return;
            }

            IsValid = true;
            Username = Convert.ToString(claims["username"]);
            Role = Convert.ToString(claims["role"]);
        }
    }
}
