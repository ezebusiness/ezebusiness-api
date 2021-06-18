using dbmongo_setup.Interfaces;
using dbmongo_setup.Models;
using Mapster;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbmongo_setup.Services
{
    public class MongoBaseService<TModel> : IMongoBaseService<TModel> where TModel : MongoBaseEntity
    {
        protected readonly IMongoGenericRepository<TModel> _genericRepository;

        public MongoBaseService(IMongoGenericRepository<TModel> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<TDto> GetById<TDto>(string id) where TDto : class
        {
            return await _genericRepository.FindOneAsync<TDto>(id);
        }

        public async Task<TModel> GetById(string id)
        {
            return await _genericRepository.FindOneAsync<TModel>(id);
        }

        public async Task<IList<TDto>> GetAll<TDto>() where TDto : class
        {
            return await _genericRepository.FindAllAsync<TDto>();
        }

        public async Task<IList<TModel>> GetAll()
        {
            return await _genericRepository.FindAllAsync<TModel>();
        }

        public async Task<IList<TDto>> Find<TDto>(Expression<Func<TModel, bool>> where) where TDto : class
        {
            return await _genericRepository.FindWhereAsync<TDto>(where);
        }

        public async Task<IList<TModel>> Find(Expression<Func<TModel, bool>> where)
        {
            return await _genericRepository.FindWhereAsync<TModel>(where);
        }

        public async Task<IList<TDto>> Find<TDto>(FilterDefinition<TModel> filterDefinition) where TDto : class
        {
            return await _genericRepository.FindWhereAsync<TDto>(filterDefinition);
        }

        public async Task<IList<TModel>> Find(FilterDefinition<TModel> filterDefinition)
        {
            return await _genericRepository.FindWhereAsync<TModel>(filterDefinition);
        }

        public BaseServiceExtension<TModel> FindWithProjection(Expression<Func<TModel, bool>> where)
        {
            return new BaseServiceExtension<TModel>(_genericRepository, where: where);
        }

        public async Task<bool> Exists(Expression<Func<TModel, bool>> expression) {
            return await _genericRepository.Exists(expression);
        }

        public async Task<string> Create<TDto>(TDto dto) where TDto : class
        {
            var model = dto.Adapt<TModel>();

            await _genericRepository.InserOneAsync(model);

            return model.Id;
        }

        public async Task<string> Create(TModel model)
        {
            await _genericRepository.InserOneAsync(model);

            return model.Id;
        }

        public async Task Create<TDto>(IList<TDto> dtos) where TDto : class
        {
            var models = dtos.Adapt<IList<TModel>>();

            await _genericRepository.InserManyAsync(models);
        }

        public async Task Create(IList<TModel> models)
        {
            await _genericRepository.InserManyAsync(models);
        }

        public async Task AddTo<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert)
        {
            await _genericRepository.AddToCollectionAsync(recordId, field, collectionsToInsert);
        }

        public async Task AddTo<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert)
        {
            await _genericRepository.AddToCollectionAsync(recordId, field, collectionToInsert);
        }

        public async Task Replace<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, TField>> field, TField collectionsToInsert)
        {
            await _genericRepository.ReplaceCollectionAsync(expresion, field, collectionsToInsert);
        }

        public async Task RemoveFrom<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove)
        {
            await _genericRepository.RemoveFromCollectionAsync(expresion, field, collectionsToRemove);
        }

        public async Task RemoveFrom<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove)
        {
            await _genericRepository.RemoveFromCollectionAsync(expresion, field, collectionToRemove);
        }

        public async Task AddTo<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert)
        {
            await _genericRepository.AddToCollectionAsync(expresion, field, collectionsToInsert);
        }

        public async Task AddTo<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert)
        {
            await _genericRepository.AddToCollectionAsync(expresion, field, collectionToInsert);
        }

        public async Task RemoveFrom<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove)
        {
            await _genericRepository.RemoveFromCollectionAsync(recordId, field, collectionsToRemove);
        }

        public async Task RemoveFrom<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove)
        {
            await _genericRepository.RemoveFromCollectionAsync(recordId, field, collectionToRemove);
        }

        public async Task Replace<TField>(string recordId, Expression<Func<TModel, TField>> field, TField collectionsToInsert)
        {
            await _genericRepository.ReplaceCollectionAsync(recordId, field, collectionsToInsert);
        }

        public async Task Delete(IList<TModel> model)
        {
            await _genericRepository.DeleteManyAsync(model);
        }

        public async Task Delete(string[] ids)
        {
            await _genericRepository.DeleteManyAsync(ids);
        }

        public async Task Delete(TModel model)
        {
            await _genericRepository.DeleteOneAsync(model);
        }

        public async Task Delete(string id)
        {
            await _genericRepository.DeleteOneAsync(id);
        }

        public async Task Upsert(TModel model)
        {
            await _genericRepository.UpsertOneAsync(model);
        }

        public async Task Upsert(TModel model, Expression<Func<TModel, bool>> expression)
        {
            await _genericRepository.UpsertOneAsync(model, expression);
        }

        public async Task Update(TModel model)
        {
            await _genericRepository.UpdateOneAsync(model);
        }

        public async Task Update<TDto>(TDto dto) where TDto : class
        {
            var model = dto.Adapt<TModel>();

            await _genericRepository.UpdateOneAsync(model);
        }

        public async Task UpdateSingleField<TField>(string id, Expression<Func<TModel, TField>> field, TField value)
        {
            await _genericRepository.UpdateOneByFieldAsync(id, field, value);
        }

        public async Task UpdateField(string id, UpdateDefinition<TModel> updateDefinition)
        {
            await _genericRepository.UpdateOneByMultipleFieldsAsync(id, updateDefinition);
        }

        public async Task UpdateField(Expression<Func<TModel, bool>> expression, UpdateDefinition<TModel> updateDefinition, bool isUpsert = false)
        {
            await _genericRepository.UpdateOneByMultipleFieldsAsync(expression, updateDefinition, isUpsert);
        }
    }
}