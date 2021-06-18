using dbmongo_setup;
using cafe_api_auth;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using common_setup;
using dbsql_setup;
using cafe_api_auth.Services;
using authentication_helper;

[assembly: FunctionsStartup(typeof(Startup))]
namespace cafe_api_auth
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            services.AddSingleton<IConfiguration>(config);

            services.AddMongoSetupDependencies(config);

            services.AddSqlSetupDependencies(config);

            services.AddAuthHelperDependencies(config);

            services.AddCommonSetupDependencies(config, typeof(UserService));
        }
    }
}