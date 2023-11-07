using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWASMOnionMessenger.Client.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientService _httpClientService;

        public AuthService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, IHttpClientService httpClientService)
        {
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _httpClientService = httpClientService;
        }
        public async Task<AuthDto> Login(LoginDto userLoginDto)
        {
            var response = await _httpClientService.PostAsync<LoginDto, AuthDto>("auth/login", userLoginDto);
            if (!response.IsSuccessful) return response;

            await _localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
            return new AuthDto { IsSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<AuthDto> Register(RegisterDto userRegisterDto)
        {
            var response = await _httpClientService.PostAsync<RegisterDto, AuthDto>("auth/register", userRegisterDto);
            if (!response.IsSuccessful) return response;

            await _localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
            return new AuthDto { IsSuccessful = true };
        }
    }
}
