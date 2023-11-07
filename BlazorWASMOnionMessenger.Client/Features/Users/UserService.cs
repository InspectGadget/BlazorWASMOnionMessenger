using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using System.Web;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;

        public UserService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<AuthDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var response = await _httpClientService.PostAsync<UserChangePasswordDto, AuthDto>("user/changepassword", userChangePasswordDto);
            if (!response.IsSuccessful) return response;
            return new AuthDto { IsSuccessful = true };
        }

        public async Task<UserDto> GetById(string userId)
        {
            return await _httpClientService.GetAsync<UserDto>("user/" + userId);
        }

        public async Task<ResponseDto> Update(UserDto userDto)
        {
            return await _httpClientService.PutAsync("user/" + userDto.Id, userDto);
        }

        public async Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            string queryString = GenerateQueryString(page, pageSize, orderBy, orderType, search);
            return await _httpClientService.GetAsync<PagedEntities<UserDto>>($"user/page?{queryString}");
        }

        private static string GenerateQueryString(int page, int pageSize, string orderBy, bool orderType, string search)
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
