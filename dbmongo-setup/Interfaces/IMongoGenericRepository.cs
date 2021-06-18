using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbmongo_setup.Interfaces
{
    public interface IMongoGenericRepository<TModel>
    {
        Task AddToCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert);
        Task AddToCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert);
        Task DeleteManyAsync(IList<TModel> model);
        Task DeleteManyAsync(string[] ids);
        Task DeleteOneAsync(string id);
        Task DeleteOneAsync(TModel model);
        Task<bool> Exists(Expression<Func<TModel, bool>> expression);
        Task<IList<TDto>> FindAllAsync<TDto>() where TDto : class;
        Task<TDto> FindOneAsync<TDto>(string id) where TDto : class;
        Task<IList<BsonDocument>> FindWhereAsync(Expression<Func<TModel, bool>> where, string[] selector, bool isInclude);
        Task<IList<TDto>> FindWhereAsync<TDto>(Expression<Func<TModel, bool>> where) where TDto : class;
        Task<IList<TDto>> FindWhereAsync<TDto>(FilterDefinition<TModel> filterDefinition) where TDto : class;
        Task InserManyAsync(IList<TModel> model);
        Task InserOneAsync(TModel model);
        Task RemoveFromCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove);
        Task RemoveFromCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove);
        Task ReplaceCollectionAsync<TField>(string recordId, Expression<Func<TModel, TField>> field, TField collectionsToInsert);
        Task UpdateOneAsync(TModel model);
        Task UpsertOneAsync(TModel model, Expression<Func<TModel, bool>> expression);
        Task UpdateOneByFieldAsync<TField>(string id, Expression<Func<TModel, TField>> field, TField value);
        Task UpdateOneByMultipleFieldsAsync(string id, UpdateDefinition<TModel> updateDefinition);
        Task UpdateOneByMultipleFieldsAsync(Expression<Func<TModel, bool>> expression, UpdateDefinition<TModel> updateDefinition, bool isUpsert = false);
        Task UpsertOneAsync(TModel model);
        Task ReplaceCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, TField>> field, TField collectionsToInsert);
        Task RemoveFromCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove);
        Task RemoveFromCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove);
        Task AddToCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert);
        Task AddToCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert);
    }
}