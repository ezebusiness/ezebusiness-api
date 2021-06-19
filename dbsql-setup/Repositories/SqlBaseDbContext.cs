using dbsql_setup.Interfaces;
using dbsql_setup.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dbsql_setup.Repositories
{
    public class SqlBaseDbContext<TModel> : DbContext where TModel : SqlBaseEntity
    {
        private readonly IConfiguration _config;

        public SqlBaseDbContext(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<TModel> GenericEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config["SqlConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<TModel>(entity =>
            //{
            //    entity.ToTable(typeof(TModel).Name);
            //});

            foreach (var mb in LoadModelBuilders())
            {
                mb.OnModelCreating(modelBuilder);
            }
        }

        private static IList<IModelBuilder> LoadModelBuilders()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());

            var mapsFrom = (
                    from type in types
                    from instance in type.GetInterfaces()
                    where
                        typeof(IModelBuilder).IsAssignableFrom(type) &&
                        !type.IsAbstract &&
                        !type.IsInterface
                    select (IModelBuilder)Activator.CreateInstance(type)).ToList();

            return mapsFrom;
        }
    }
}