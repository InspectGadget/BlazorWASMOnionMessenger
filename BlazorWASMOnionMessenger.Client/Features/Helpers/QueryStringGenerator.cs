using System.Web;

namespace BlazorWASMOnionMessenger.Client.Features.Helpers
{
    public class QueryStringGenerator
    {
        public static string GenerateQueryString(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            var queryParameters = new System.Collections.Specialized.NameValueCollection();

            queryParameters["page"] = page.ToString();
            queryParameters["pageSize"] = pageSize.ToString();
            queryParameters["orderBy"] = orderBy;
            queryParameters["orderType"] = orderType.ToString();
            queryParameters["search"] = search;

            var queryString = string.Join("&",
                queryParameters.AllKeys.Select(key =>
                    $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(queryParameters[key])}"
                )
            );

            return queryString;
        }
    }
}
