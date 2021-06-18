using dbmongo_setup.Models;
using dbmongo_setup.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbmongo_setup.Interfaces
{
    public interface IMongoBaseService<TModel> where TModel : MongoBaseEntity
    {
        Task AddTo<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert);
        Task AddTo<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert);
        Task Create(IList<TModel> models);
        Task<string> Create(TModel model);
        Task Create<TDto>(IList<TDto> dtos) where TDto : class;
        Task<string> Create<TDto>(TDto dto) where TDto : class;
        Task Delete(IList<TModel> model);
        Task Delete(string id);
        Task Delete(string[] ids);
        Task Delete(TModel model);
        Task<bool> Exists(Expression<Func<TModel, bool>> expression);
        Task<IList<TModel>> Find(Expression<Func<TModel, bool>> where);
        Task<IList<TModel>> Find(FilterDefinition<TModel> filterDefinition);
        Task<IList<TDto>> Find<TDto>(Expression<Func<TModel, bool>> where) where TDto : class;
        Task<IList<TDto>> Find<TDto>(FilterDefinition<TModel> filterDefinition) where TDto : class;
        BaseServiceExtension<TModel> FindWithProjection(Expression<Func<TModel, bool>> where);
        Task<IList<TModel>> GetAll();
        Task<IList<TDto>> GetAll<TDto>() where TDto : class;
        Task<TModel> GetById(string id);
        Task<TDto> GetById<TDto>(string id) where TDto : class;
        Task RemoveFrom<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove);
        Task RemoveFrom<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove);
        Task AddTo<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert);
        Task AddTo<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert);
        Task RemoveFrom<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove);
        Task RemoveFrom<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove);
        Task Replace<TField>(string recordId, Expression<Func<TModel, TField>> field, TField collectionsToInsert);
        Task Replace<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, TField>> field, TField collectionsToInsert);
        Task Update(TModel model);
        Task Upsert(TModel model, Expression<Func<TModel, bool>> expression);
        Task Update<TDto>(TDto dto) where TDto : class;
        Task UpdateField(string id, UpdateDefinition<TModel> updateDefinition);
        Task UpdateField(Expression<Func<TModel, bool>> expression, UpdateDefinition<TModel> updateDefinition, bool isUpsert = false);
        Task UpdateSingleField<TField>(string id, Expression<Func<TModel, TField>> field, TField value);
        Task Upsert(TModel model);
    }
}