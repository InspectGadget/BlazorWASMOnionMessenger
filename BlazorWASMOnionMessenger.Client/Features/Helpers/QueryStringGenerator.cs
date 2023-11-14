using System.Web;

namespace BlazorWASMOnionMessenger.Client.Features.Helpers
{
    public class QueryStringGenerator
    {
        public static string GenerateGridQueryString(int page, int pageSize, string orderBy, bool orderType, string search)
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
        public static string GenerateMessageQueryString(string userId, int chatId, int quantity, int skip)
        {
            var queryParameters = new System.Collections.Specialized.NameValueCollection();

            queryParameters["userId"] = userId;
            queryParameters["chatId"] = chatId.ToString();
            queryParameters["quantity"] = quantity.ToString();
            queryParameters["skip"] = skip.ToString();

            var queryString = string.Join("&",
                queryParameters.AllKeys.Select(key =>
                    $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(queryParameters[key])}"
                )
            );

            return queryString;
        }
    }
}
