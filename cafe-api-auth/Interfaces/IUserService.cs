using authentication_helper;
using cafe_api_auth.Models;
using cafe_common.Models;
using common_setup;
using dbmongo_setup.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cafe_api_auth.Interfaces
{
    public interface IUserService : IMongoBaseService<User>, IInjectable
    {
        Task<string> RenewAccessToken(HttpRequest req);
        Task RevokeAccessToken(HttpRequest req);
        Task UpsertUser(AuthRequest loginRequest, AuthResponse<Customer> loginResponse);
    }
}