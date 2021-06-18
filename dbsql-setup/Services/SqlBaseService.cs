using dbsql_setup.Interfaces;
using dbsql_setup.Models;
using dbsql_setup.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace dbsql_setup.Services
{
    public class SqlBaseService<TModel> : ISqlBaseService<TModel> where TModel : SqlBaseEntity
    {
        private readonly ISqlGenericRepository<TModel> _genericRepository;

        public SqlBaseService(ISqlGenericRepository<TModel> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddAsync(TModel modelToAdd)
        {
            await _genericRepository.AddAsync(modelToAdd);
        }

        public void Delete(TModel modelToDelete)
        {
            _genericRepository.Delete(modelToDelete);
        }

        public void DeleteById(int id)
        {
            _genericRepository.DeleteById(id);
        }

        public void DeleteRange(IList<TModel> modelsToDelete)
        {
            _genericRepository.DeleteRange(modelsToDelete);
        }

        public bool Exist(Expression<Func<TModel, bool>> predicate)
        {
            return _genericRepository.Exist(predicate);
        }

        public IList<TModel> Find(Func<TModel, bool> predicate) {
            return _genericRepository.Find(predicate);
        }

        public async Task<IList<TModel>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public TModel GetById(int id)
        {
            return _genericRepository.GetById(id);
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _genericRepository.SaveChangesAsync();
        }

        public void Update(TModel modelToUpdate)
        {
            _genericRepository.Update(modelToUpdate);
        }

        public void UpdateRange(IList<TModel> modelsToUpdate)
        {
            _genericRepository.UpdateRange(modelsToUpdate);
        }
    }
}
