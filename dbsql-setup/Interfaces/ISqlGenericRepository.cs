using dbsql_setup.Models;
using dbsql_setup.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbsql_setup.Interfaces
{
    public interface ISqlGenericRepository<TModel> where TModel : SqlBaseEntity
    {
        Task AddAsync(TModel modelToAdd);
        Task AddRangeAsync(IList<TModel> modelsToAdd);
        void Delete(TModel modelToDelete);
        void DeleteById(int id);
        void DeleteRange(IList<TModel> modelsToDelete);
        bool Exist(Expression<Func<TModel, bool>> predicate);
        Task<IList<TModel>> GetAllAsync();
        IList<TModel> Find(Func<TModel, bool> predicate);
        TModel GetById(int id);
        Task<TModel> GetByIdAsync(int id);
        Task<int> SaveChangesAsync();
        void Update(TModel modelToUpdate);
        void UpdateRange(IList<TModel> modelsToUpdate);
        IList<TModel> Find<TChild>(Func<TModel, bool> predicate, Expression<Func<TModel, TChild>> includeObject);
        IList<TModel> Find(string sqlStatement, params object[] parameters);
        SqlBaseDbContext<TModel> DbContext();
        DbSet<TModel> DbSet();
    }
}