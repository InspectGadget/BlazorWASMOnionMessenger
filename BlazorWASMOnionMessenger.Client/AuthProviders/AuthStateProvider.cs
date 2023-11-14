using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.HttpServices;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.Client.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpClientService httpClientService;
        private readonly ILocalStorageService localStorage;
        private readonly JwtTokenParser jwtTokenParser;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(IHttpClientService httpClientService, ILocalStorageService localStorage, JwtTokenParser jwtTokenParser)
        {
            this.httpClientService = httpClientService;
            this.localStorage = localStorage;
            this.jwtTokenParser = jwtTokenParser;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return anonymous;
            httpClientService.SetAuthorizationHeader(token);
            var claims = jwtTokenParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyUserAuthentication(string token)
        {
            httpClientService.SetAuthorizationHeader(token);
            var claims = jwtTokenParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            httpClientService.RemoveAuthorizationHeader();
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
