using dbsql_setup.Interfaces;
using dbsql_setup.Repositories;
using dbsql_setup.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dbsql_setup
{
    public static class SqlStartupExtension
    {
        #region Methods
        public static IServiceCollection AddSqlSetupDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(ISqlGenericRepository<>), typeof(SqlGenericRepository<>));
            services.AddTransient(typeof(ISqlBaseService<>), typeof(SqlBaseService<>));

            return services;
        }
        #endregion
    }
}
