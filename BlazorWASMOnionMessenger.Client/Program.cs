using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Client.HttpServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

await builder.Build().RunAsync();
