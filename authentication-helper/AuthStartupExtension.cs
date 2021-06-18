using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace authentication_helper
{
    public static class AuthStartupExtension
{
        public static IServiceCollection AddAuthHelperDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(ITokenHelper), typeof(TokenHelper));
            services.AddTransient(typeof(IEncryptionHelper), typeof(EncryptionHelper));
            services.AddTransient(typeof(IUserHelper), typeof(UserHelper));

            return services;
        }
    }
}
