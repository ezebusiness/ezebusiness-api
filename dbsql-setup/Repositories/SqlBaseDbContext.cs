using dbsql_setup.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

            modelBuilder.Entity<TModel>(entity =>
            {
                entity.ToTable(typeof(TModel).Name);
            });
        }
    }
}