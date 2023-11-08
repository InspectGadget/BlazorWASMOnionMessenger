using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using LinqKit;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorWASMOnionMessenger.Application.Common
{
    public class SearchPredicateBuilder : ISearchPredicateBuilder
    {
        private const string EntityParameterName = "entity";
        private const string ContainsMethodName = "Contains";
        public Expression<Func<T, bool>> BuildSearchPredicate<T, TDTO>(string search)
        {
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var predicate = PredicateBuilder.New<T>(false);
            
            var properties = typeof(TDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                foreach (var keyword in keywords)
                {
                    var containsMethod = typeof(string).GetMethod(ContainsMethodName, new[] { typeof(string) });
                    var entity = Expression.Parameter(typeof(T), EntityParameterName);
                    var propertyAccessor = Expression.PropertyOrField(entity, property.Name);
                    var searchWordConstant = Expression.Constant(keyword);
                    var containsCall = Expression.Call(propertyAccessor, containsMethod, searchWordConstant);
                    var lambdaPredicate = Expression.Lambda<Func<T, bool>>(containsCall, entity);
                    predicate = predicate.Or(lambdaPredicate);
                }
            }
            return predicate;
        }

        
    }
}
