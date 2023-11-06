using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using System.Web;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientService _httpClient;

        public UserService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, IHttpClientService httpClientService)
        {
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _httpClient = httpClientService;
        }

        public async Task<UserAuthDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var response = await _httpClient.PostAsync<UserChangePasswordDto, UserAuthDto>("user/changepassword", userChangePasswordDto);
            if (!response.IsSuccessful) return response;
            return new UserAuthDto { IsSuccessful = true };
        }

        public async Task<UserDto> GetById(string userId)
        {
            return await _httpClient.GetAsync<UserDto>("user/" + userId);
        }

        public async Task<ResponseDto> Update(UserDto userDto)
        {
            return await _httpClient.PutAsync("user/" + userDto.Id, userDto);
        }

        public async Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            string queryString = GenerateQueryString(page, pageSize, orderBy, orderType, search);
            return await _httpClient.GetAsync<PagedEntities<UserDto>>($"user/page?{queryString}");
        }

        public async Task<UserAuthDto> Login(UserLoginDto userLoginDto)
        {
            var response = await _httpClient.PostAsync<UserLoginDto, UserAuthDto>("user/login", userLoginDto);

            if (!response.IsSuccessful) return response;

            await _localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
            return new UserAuthDto { IsSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<UserAuthDto> Register(UserRegisterDto userRegisterDto)
        {
            var response = await _httpClient.PostAsync<UserRegisterDto, UserAuthDto>("user/register", userRegisterDto);

            if (!response.IsSuccessful) return response;

            await _localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
            return new UserAuthDto { IsSuccessful = true };
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
