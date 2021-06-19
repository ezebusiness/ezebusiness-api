using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using authentication_helper;
using cafe_common.Models;
using cafe_api_auth.Interfaces;
using System.IO;
using Newtonsoft.Json;
using Mapster;
using cafe_api_auth.Dtos;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Linq;

namespace cafe_api_auth
{
    public class UserAuthFunction
    {
        private readonly ILogger<User> _logger;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;

        public UserAuthFunction(
            ILogger<User> logger,
            IUserService userService,
            ICustomerService customerService)
        {
            _logger = logger;
            _userService = userService;
            _customerService = customerService;
        }

        [FunctionName("CafeAuthLogin")]
        public async Task<HttpResponseMessage> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] HttpRequest req)
        {
            try
            {
                var reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                var loginRequest = JsonConvert.DeserializeObject<AuthRequest>(reqBody);
                var loginResponse = _customerService.LoginUser(loginRequest, req);

                if (!loginResponse.Success)
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);

                await _userService.UpsertUser(loginRequest, loginResponse);

                var res = new HttpResponseMessage(HttpStatusCode.OK);

                var roles = loginResponse.User.CustomerRoleMappings.Select(q => new { q.CustomerRole.Id, q.CustomerRole.Name });

                res.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    user = loginResponse.User.Adapt<CustomerDto>(),
                    roles = roles,
                    access_token = loginResponse.Token,
                    refresh_token = loginResponse.RefreshToken
                }), Encoding.UTF8, "application/json");

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.StackTrace}");

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [FunctionName("CafeAuthRefresh")]
        public async Task<HttpResponseMessage> RefreshAccessToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/refresh")] HttpRequest req)
        {
            try
            {
                var newAccessToken = await _userService.RenewAccessToken(req);

                if (String.IsNullOrEmpty(newAccessToken))
                    throw new NullReferenceException("access token is not valid or expired.");

                var res = new HttpResponseMessage(HttpStatusCode.OK);

                res.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    access_token = newAccessToken
                }), Encoding.UTF8, "application/json");

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [FunctionName("CafeAuthLogout")]
        public async Task<IActionResult> Logout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/logout")] HttpRequest req)
        {
            try
            {
                await _userService.RevokeAccessToken(req);

                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        [FunctionName("CafeAuthGetUser")]
        public IActionResult GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auth/user")] HttpRequest req)
        {
            try
            {
                var user = _customerService.GetUser(req);

                return new OkObjectResult(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }
    }
}
