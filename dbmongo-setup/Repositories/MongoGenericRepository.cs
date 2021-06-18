using dbmongo_setup.Interfaces;
using dbmongo_setup.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBServerSideProjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbmongo_setup.Repositories
{
    public class MongoGenericRepository<TModel> : IMongoGenericRepository<TModel> where TModel : MongoBaseEntity
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TModel> _entity;

        public MongoGenericRepository(MongoClient mongoClient, IConfiguration configuration)
        {
            _database = mongoClient.GetDatabase(configuration["DatabaseName"]);
            _entity = _database.GetCollection<TModel>(typeof(TModel).Name.ToPluralizedCamelCase());
        }

        public async Task<TDto> FindOneAsync<TDto>(string id) where TDto : class
        {
            var filter = Builders<TModel>.Filter.Eq(n => n.Id, id);

            return await _entity.Find(filter).ProjectTo<TModel, TDto>().FirstOrDefaultAsync();
        }

        public async Task<IList<TDto>> FindWhereAsync<TDto>(Expression<Func<TModel, bool>> where) where TDto : class
        {
            var filter = Builders<TModel>.Filter.Where(where);

            return await _entity.Find(filter).ProjectTo<TModel, TDto>().ToListAsync();
        }

        public async Task<IList<BsonDocument>> FindWhereAsync(Expression<Func<TModel, bool>> where, string[] selector, bool isInclude)
        {
            var filter = Builders<TModel>.Filter.Where(where);

            if (isInclude)
            {
                var projection = Builders<TModel>.Projection.Include("");

                selector.ToList().ForEach(q =>
                {
                    projection = projection.Include(q);
                });

                return await _entity.Find(filter).Project<BsonDocument>(projection).ToListAsync();
            }
            else
            {
                var projection = Builders<TModel>.Projection.Exclude("");

                selector.ToList().ForEach(q =>
                {
                    projection = projection.Exclude(q);
                });

                return await _entity.Find(filter).Project<BsonDocument>(projection).ToListAsync();
            }
        }

        public async Task<IList<TDto>> FindWhereAsync<TDto>(FilterDefinition<TModel> filterDefinition) where TDto : class
        {
            return await _entity.Find(filterDefinition).ProjectTo<TModel, TDto>().ToListAsync();
        }

        public async Task<IList<TDto>> FindAllAsync<TDto>() where TDto : class
        {
            var filter = Builders<TModel>.Filter.Where(q => true);

            return await _entity.Find(filter).ProjectTo<TModel, TDto>().ToListAsync();
        }

        public async Task InserOneAsync(TModel model)
        {
            model.CreatedAtUtc = DateTime.UtcNow;
            model.UpdatedAtUtc = DateTime.UtcNow;

            await _entity.InsertOneAsync(model);
        }

        public async Task InserManyAsync(IList<TModel> model)
        {
            model.ToList().ForEach(q =>
            {
                q.UpdatedAtUtc = DateTime.UtcNow;
                q.CreatedAtUtc = DateTime.UtcNow;
            });

            await _entity.InsertManyAsync(model);
        }

        public async Task AddToCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Where(expresion);
                var update = Builders<TModel>.Update.AddToSet(member.Member.Name, collectionToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task AddToCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Where(expresion);
                var update = Builders<TModel>.Update.AddToSetEach(member.Member.Name, collectionsToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task RemoveFromCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Where(expresion);
                var update = Builders<TModel>.Update.Pull(member.Member.Name, collectionToRemove)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task RemoveFromCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Where(expresion);
                var update = Builders<TModel>.Update.PullAll(member.Member.Name, collectionsToRemove)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task ReplaceCollectionAsync<TField>(Expression<Func<TModel, bool>> expresion, Expression<Func<TModel, TField>> field, TField collectionsToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Where(expresion);
                var update = Builders<TModel>.Update.Set(member.Member.Name, collectionsToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task AddToCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, recordId);
                var update = Builders<TModel>.Update.AddToSet(member.Member.Name, collectionToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task AddToCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, recordId);
                var update = Builders<TModel>.Update.AddToSetEach(member.Member.Name, collectionsToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task RemoveFromCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, TField collectionToRemove)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, recordId);
                var update = Builders<TModel>.Update.Pull(member.Member.Name, collectionToRemove)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task RemoveFromCollectionAsync<TField>(string recordId, Expression<Func<TModel, IEnumerable<TField>>> field, IEnumerable<TField> collectionsToRemove)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, recordId);

                var update = Builders<TModel>.Update.PullAll(member.Member.Name, collectionsToRemove)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task ReplaceCollectionAsync<TField>(string recordId, Expression<Func<TModel, TField>> field, TField collectionsToInsert)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, recordId);
                var update = Builders<TModel>.Update.Set(member.Member.Name, collectionsToInsert)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task DeleteOneAsync(TModel model)
        {
            var filter = Builders<TModel>.Filter.Eq(n => n.Id, model.Id);

            await _entity.DeleteOneAsync(filter);
        }

        public async Task DeleteOneAsync(string id)
        {
            await _entity.DeleteOneAsync(q => q.Id == id);
        }

        public async Task DeleteManyAsync(IList<TModel> model)
        {
            var ids = model.Select(q => q.Id).ToArray();

            await _entity.DeleteManyAsync(q => ids.Contains(q.Id));
        }

        public async Task DeleteManyAsync(string[] ids)
        {
            await _entity.DeleteManyAsync(q => ids.Contains(q.Id));
        }

        public async Task UpdateOneAsync(TModel model)
        {
            var filter = Builders<TModel>.Filter.Eq(n => n.Id, model.Id);
            model.UpdatedAtUtc = DateTime.UtcNow;

            await _entity.ReplaceOneAsync(filter, model);
        }

        public async Task UpsertOneAsync(TModel model)
        {
            var options = new ReplaceOptions() { IsUpsert = true };
            var filter = Builders<TModel>.Filter.Eq(n => n.Id, model.Id);
            model.UpdatedAtUtc = DateTime.UtcNow;

            await _entity.ReplaceOneAsync(filter, model, options);
        }

        public async Task UpsertOneAsync(TModel model, Expression<Func<TModel, bool>> expression)
        {
            var options = new ReplaceOptions() { IsUpsert = true };
            var filter = Builders<TModel>.Filter.Where(expression);
            model.UpdatedAtUtc = DateTime.UtcNow;

            await _entity.ReplaceOneAsync(filter, model, options);
        }

        public async Task UpdateOneByFieldAsync<TField>(string id, Expression<Func<TModel, TField>> field, TField value)
        {
            var member = field.Body as MemberExpression;

            if (member != null)
            {
                var filter = Builders<TModel>.Filter.Eq(n => n.Id, id);
                var update = Builders<TModel>.Update.Set(member.Member.Name, value)
                                                    .Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

                await _entity.UpdateOneAsync(filter, update);
            }
        }

        public async Task UpdateOneByMultipleFieldsAsync(Expression<Func<TModel, bool>> expression, UpdateDefinition<TModel> updateDefinition, bool isUpsert = false)
        {
            var options = new UpdateOptions() { IsUpsert = isUpsert };
            var filter = Builders<TModel>.Filter.Where(expression);
            updateDefinition.Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

            await _entity.UpdateOneAsync(filter, updateDefinition, options);
        }

        public async Task UpdateOneByMultipleFieldsAsync(string id, UpdateDefinition<TModel> updateDefinition)
        {
            var filter = Builders<TModel>.Filter.Eq(n => n.Id, id);

            updateDefinition.Set(q => q.UpdatedAtUtc, DateTime.UtcNow);

            await _entity.UpdateOneAsync(filter, updateDefinition);
        }

        public async Task<bool> Exists(Expression<Func<TModel, bool>> expression)
        {
            var filter = Builders<TModel>.Filter.Where(expression);
            var count = await _entity.Find(filter).CountDocumentsAsync();

            return count > 0;
        }
    }

    public static class StringExtensions
    {
        public static string ToPluralizedCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1) + "s";
        }
    }
}
