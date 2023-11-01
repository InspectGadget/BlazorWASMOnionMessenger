using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.HttpServices;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.Client.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(IHttpClientService httpClientService, ILocalStorageService localStorage)
        {
            _httpClientService = httpClientService;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;
            _httpClientService.SetAuthorizationHeader(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtTokenParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string token)
        {
            _httpClientService.SetAuthorizationHeader(token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtTokenParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            _httpClientService.RemoveAuthorizationHeader();
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
