using dbsql_setup.Interfaces;
using dbsql_setup.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbsql_setup.Repositories
{
    public class SqlGenericRepository<TModel> : ISqlGenericRepository<TModel> where TModel : SqlBaseEntity
    {
        private readonly DbSet<TModel> _dbSet;
        private readonly SqlBaseDbContext<TModel> _dbContext;

        public SqlGenericRepository(IConfiguration config)
        {
            _dbContext = new SqlBaseDbContext<TModel>(config);
            _dbSet = _dbContext.GenericEntity;
        }

        public SqlBaseDbContext<TModel> DbContext()
        {
            return _dbContext;
        }

        public DbSet<TModel> DbSet()
        {
            return _dbSet;
        }

        public async Task<IList<TModel>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IList<TModel> Find(string sqlStatement, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sqlStatement, parameters).ToList();
        }

        public IList<TModel> Find(Func<TModel, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IList<TModel> Find<TChild>(Func<TModel, bool> predicate, Expression<Func<TModel, TChild>> includeObject)
        {
            return _dbSet.Include(includeObject).Where(predicate).ToList();
        }

        public TModel GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TModel modelToAdd)
        {
            await _dbContext.AddAsync(modelToAdd);
        }

        public async Task AddRangeAsync(IList<TModel> modelsToAdd)
        {
            await _dbContext.AddRangeAsync(modelsToAdd);
        }

        public void Update(TModel modelToUpdate)
        {
            _dbContext.Update(modelToUpdate);
        }

        public void UpdateRange(IList<TModel> modelsToUpdate)
        {
            _dbContext.UpdateRange(modelsToUpdate);
        }

        public void DeleteById(int id)
        {
            var entityToDelete = GetById(id);

            _dbContext.Remove(entityToDelete);
        }

        public void Delete(TModel modelToDelete)
        {
            _dbContext.Remove(modelToDelete);
        }

        public void DeleteRange(IList<TModel> modelsToDelete)
        {
            _dbContext.RemoveRange(modelsToDelete);
        }

        public bool Exist(Expression<Func<TModel, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
