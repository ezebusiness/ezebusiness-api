using dbmongo_setup.Interfaces;
using dbmongo_setup.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dbmongo_setup.Services
{
    public interface IBaseServiceExtension<T> where T : class
    {
        public Task<IList<TResult>> Include<TResult>(Expression<Func<T, TResult>> expression);
        public Task<IList<BsonDocument>> Exclude<TResult>(Expression<Func<T, TResult>> expression);
        public Task<IList<BsonDocument>> Include(params Expression<Func<T, object>>[] expressions);
        public Task<IList<BsonDocument>> Exclude(params Expression<Func<T, object>>[] expressions);
    }

    public class BaseServiceExtension<T> : IBaseServiceExtension<T> where T : MongoBaseEntity
    {
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";
        private readonly IMongoGenericRepository<T> _genericRepository;
        private readonly Expression<Func<T, bool>> _where;

        public BaseServiceExtension(IMongoGenericRepository<T> genericRepository, Expression<Func<T, bool>> where) {
            _where = where;
            _genericRepository = genericRepository;
        }

        public async Task<IList<TResult>> Include<TResult>(Expression<Func<T, TResult>> expression)
        {
            string[] selectorArray = { GetMemberName(expression.Body) };
            var results = await _genericRepository.FindWhereAsync(_where, selectorArray, true);

            var returnList = new List<TResult>();
            foreach (var res in results)
            {
                var resultParameterType = typeof(TResult);

                if (resultParameterType.IsGenericType)
                {
                    var bsonArray = res[selectorArray[0]].AsBsonArray;
                    var desirializedJson = JsonConvert.DeserializeObject<TResult>(bsonArray.ToJson());

                    returnList.Add(desirializedJson);
                }
                else
                {
                    var desirializedJson = JsonConvert.DeserializeObject<TResult>(res[selectorArray[0]].ToJson());

                    returnList.Add(desirializedJson);
                }
            }

            return returnList;
        }

        public async Task<IList<BsonDocument>> Exclude<TResult>(Expression<Func<T, TResult>> expression)
        {
            string[] selectorArray = { GetMemberName(expression.Body) };

            return await _genericRepository.FindWhereAsync(_where, selectorArray, false);
        }

        public async Task<IList<BsonDocument>> Include(params Expression<Func<T, object>>[] expressions)
        {
            var memberNames = new List<string>();

            foreach (var cExpression in expressions)
                memberNames.Add(GetMemberName(cExpression.Body));

            var selectorArray = memberNames.ToArray();

            return await _genericRepository.FindWhereAsync(_where, selectorArray, true);
        }

        public async Task<IList<BsonDocument>> Exclude(params Expression<Func<T, object>>[] expressions)
        {
            List<string> memberNames = new List<string>();

            foreach (var cExpression in expressions)
                memberNames.Add(GetMemberName(cExpression.Body));

            var selectorArray = memberNames.ToArray();

            return await _genericRepository.FindWhereAsync(_where, selectorArray, false);
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }
            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }
            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }
            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }
            throw new ArgumentException(invalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }
            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }
}
