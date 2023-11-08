using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Common
{
    public interface ISearchPredicateBuilder
    {
        Expression<Func<T, bool>> BuildSearchPredicate<T, TDTO>(string search);
    }
}
