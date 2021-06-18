using dbmongo_setup.Interfaces;
using dbmongo_setup.Repositories;
using dbmongo_setup.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Security.Authentication;

namespace dbmongo_setup
{
    public static class MongoStartupExtension
    {
        #region Methods
        public static IServiceCollection AddMongoSetupDependencies(this IServiceCollection services, IConfiguration config)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(config["MongoConnectionString"]));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            services.AddSingleton((s) => new MongoClient(settings));

            services.AddTransient(typeof(IMongoGenericRepository<>), typeof(MongoGenericRepository<>));
            services.AddTransient(typeof(IMongoBaseService<>), typeof(MongoBaseService<>));

            return services;
        }
        #endregion
    }
}