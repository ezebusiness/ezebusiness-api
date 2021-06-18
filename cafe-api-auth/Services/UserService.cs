using authentication_helper;
using cafe_api_auth.Interfaces;
using cafe_api_auth.Models;
using cafe_common.Models;
using dbmongo_setup.Interfaces;
using dbmongo_setup.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cafe_api_auth.Services
{
    public class UserService : MongoBaseService<User>, IUserService
    {
        private readonly IUserHelper _userHelper;
        private readonly ITokenHelper _tokenHelper;

        public UserService(IMongoGenericRepository<User> userRepository,
            IUserHelper userHelper,
            ITokenHelper tokenHelper) : base(userRepository)
        {
            _userHelper = userHelper;
            _tokenHelper = tokenHelper;
        }

        public async Task UpsertUser(AuthRequest loginRequest, AuthResponse<Customer> loginResponse)
        {
            var userModel = new User()
            {
                Username = loginRequest.Username,
                RefreshTokens = new string[] { loginResponse.RefreshToken }
            };

            var options = new UpdateOptions() { IsUpsert = true };

            var update = Builders<User>.Update.AddToSetEach(q => q.RefreshTokens, userModel.RefreshTokens)
                                              .Set(q => q.Username, userModel.Username)
                                              .Set(q => q.UpdatedAtUtc, DateTime.UtcNow)
                                              .Set(q => q.CreatedAtUtc, DateTime.UtcNow);

            await UpdateField(q => q.Username == userModel.Username, update, isUpsert: true);
        }

        public async Task<string> RenewAccessToken(HttpRequest req)
        {
            var refreshToken = req.HttpContext.Request.Cookies["auth._refresh_token.local"];

            var tokenValidation = _tokenHelper.ValidateJwtToken(refreshToken);

            if (tokenValidation == null)
                return null;

            var username = _userHelper.GetCurrentUserIdentity(req);

            var user = (await Find(q => q.Username == username)).FirstOrDefault();

            if (user != null)
            {
                bool isValid = user.RefreshTokens.Contains(refreshToken);

                if (isValid)
                    return _tokenHelper.GenerateAccessToken(username);
            }

            return null;
        }

        public async Task RevokeAccessToken(HttpRequest req)
        {
            var refreshToken = req.HttpContext.Request.Cookies["auth._refresh_token.local"];

            var tokenValidation = _tokenHelper.ValidateJwtToken(refreshToken);

            if (tokenValidation != null)
            {
                var username = _userHelper.GetCurrentUserIdentity(req);

                var user = (await Find(q => q.Username == username)).FirstOrDefault();

                if (user != null)
                {
                    bool isValid = user.RefreshTokens.Contains(refreshToken);

                    if (isValid)
                        await RemoveFrom(user.Id, q => q.RefreshTokens, refreshToken);
                }
            }
        }
    }
}
