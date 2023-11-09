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
            if (string.IsNullOrEmpty(search))
            {
                return PredicateBuilder.New<T>(true);
            }
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var predicate = PredicateBuilder.New<T>(false);

            var propertiesInT = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.PropertyType == typeof(string));

            var properties = typeof(TDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propDTO => propDTO.PropertyType == typeof(string) &&
                                  propertiesInT.Any(propT => propT.Name == propDTO.Name))
                .ToList();

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
