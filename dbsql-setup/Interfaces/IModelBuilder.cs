using Microsoft.EntityFrameworkCore;

namespace dbsql_setup.Interfaces
{
    public interface IModelBuilder
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
