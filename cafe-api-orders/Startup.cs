using dbmongo_setup;
using cafe_api_orders;
using cafe_api_orders.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using common_setup;
using dbsql_setup;

[assembly: FunctionsStartup(typeof(Startup))]
namespace cafe_api_orders
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

            services.AddCommonSetupDependencies(config, typeof(BookService));
        }
    }
}