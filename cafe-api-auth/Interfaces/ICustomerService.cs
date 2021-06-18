using authentication_helper;
using cafe_api_auth.Dtos;
using cafe_api_auth.Models;
using common_setup;
using dbsql_setup.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cafe_api_auth.Interfaces
{
    public interface ICustomerService : ISqlBaseService<Customer>, IInjectable
    {
        CustomerDto GetUser(HttpRequest req);
        AuthResponse<Customer> LoginUser(AuthRequest loginRequest, HttpRequest req);
    }
}