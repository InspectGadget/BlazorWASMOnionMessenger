using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorWASMOnionMessenger.Client.HttpServices;

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

        public async Task<UserResponseDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var result = await _httpClient.PostAsync<UserChangePasswordDto, UserResponseDto>("user/changepassword", userChangePasswordDto);
            if (!result.IsSuccessful) return result;
            return new UserResponseDto { IsSuccessful = true };
        }

        public async Task<UserResponseDto> Login(UserLoginDto userLoginDto)
        {
            var result = await _httpClient.PostAsync<UserLoginDto, UserResponseDto>("user/login", userLoginDto);

            if (!result.IsSuccessful) return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            return new UserResponseDto { IsSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<UserResponseDto> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _httpClient.PostAsync<UserRegisterDto, UserResponseDto>("user/register", userRegisterDto);

            if (!result.IsSuccessful) return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            return new UserResponseDto { IsSuccessful = true };
        }
    }
}
