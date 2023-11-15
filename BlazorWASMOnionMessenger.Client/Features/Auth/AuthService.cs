using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWASMOnionMessenger.Client.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;
        private readonly IHttpClientService httpClientService;

        public AuthService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, IHttpClientService httpClientService)
        {
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;
            this.httpClientService = httpClientService;
        }
        public async Task<AuthDto> Login(LoginDto userLoginDto)
        {
            var response = await httpClientService.PostAsync<LoginDto, AuthDto>("auth/login", userLoginDto);
            if (!response.IsSuccessful) return response;

            await localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(response.Token);
            return new AuthDto { IsSuccessful = true };
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
        }

        public async Task<AuthDto> Register(RegisterDto userRegisterDto)
        {
            var response = await httpClientService.PostAsync<RegisterDto, AuthDto>("auth/register", userRegisterDto);
            if (!response.IsSuccessful) return response;

            await localStorage.SetItemAsync("authToken", response.Token);
            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(response.Token);
            return new AuthDto { IsSuccessful = true };
        }
    }
}
