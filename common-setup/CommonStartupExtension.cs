using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace common_setup
{
    public static class CommonStartupExtension
    {
        #region Methods
        public static IServiceCollection AddCommonSetupDependencies(this IServiceCollection services, IConfiguration config, Type objectType)
        {
            services.AddIInjectableDependencies(objectType);

            return services;
        }
        #endregion
    }
}
