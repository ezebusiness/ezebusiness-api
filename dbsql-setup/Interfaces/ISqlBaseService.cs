using dbsql_setup.Models;
using dbsql_setup.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbsql_setup.Interfaces
{
    public interface ISqlBaseService<TModel> where TModel : SqlBaseEntity
    {
        Task AddAsync(TModel modelToAdd);
        SqlBaseDbContext<TModel> DbContext();
        DbSet<TModel> DbSet();
        void Delete(TModel modelToDelete);
        void DeleteById(int id);
        void DeleteRange(IList<TModel> modelsToDelete);
        bool Exist(Expression<Func<TModel, bool>> predicate);
        IList<TModel> Find<TChild>(Func<TModel, bool> predicate, Expression<Func<TModel, TChild>> includeObject);
        IList<TModel> Find(string sqlStatement, params object[] parameters);
        Task<IList<TModel>> GetAllAsync();
        TModel GetById(int id);
        Task<TModel> GetByIdAsync(int id);
        Task<int> SaveChangesAsync();
        void Update(TModel modelToUpdate);
        void UpdateRange(IList<TModel> modelsToUpdate);
    }
}