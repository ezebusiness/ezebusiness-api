using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace authentication_helper
{
    public interface ITokenHelper
    {
        string GenerateAccessToken(string username);
        string GenerateRefreshToken(HttpRequest req);
        string GetUserIpAddress(HttpRequest req);
        HttpResponseMessage SetRefreshTokenCookie(string refreshToken, HttpRequest req);
        JwtSecurityToken ValidateJwtToken(string token);
    }

    public class TokenHelper : ITokenHelper
    {
        public string GetUserIpAddress(HttpRequest req)
        {
            if (req.Headers.ContainsKey("X-Forwarded-For"))
                return req.Headers["X-Forwarded-For"];
            else
                return req.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public string GenerateAccessToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtConfigurations.SECRET_KEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "[]"),
                }),

                Expires = DateTime.UtcNow.AddMinutes(JwtConfigurations.TOKEN_EXPIRY_IN_MINUTES),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(HttpRequest req)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtConfigurations.SECRET_KEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("ip_address", GetUserIpAddress(req))
                }),
                Expires = DateTime.UtcNow.AddMinutes(JwtConfigurations.REFRESH_EXPIRY_IN_DAYS),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken ValidateJwtToken(string token)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfigurations.SECRET_KEY));

            var tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = signingKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken validatedToken;

                tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                return tokenHandler.ReadJwtToken(token);
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage SetRefreshTokenCookie(string refreshToken, HttpRequest req) {
            var rehreshCookie = new CookieHeaderValue("refresh_token", refreshToken);

            rehreshCookie.Expires = DateTime.UtcNow.AddMinutes(JwtConfigurations.REFRESH_EXPIRY_IN_DAYS);
            rehreshCookie.HttpOnly = true;
            rehreshCookie.Path = "/";
            rehreshCookie.Domain = req.HttpContext.Request.Host.Value;

            var res = new HttpResponseMessage(HttpStatusCode.OK);

            res.Headers.AddCookies(new CookieHeaderValue[] { rehreshCookie });

            return res;
        }
    }
}
