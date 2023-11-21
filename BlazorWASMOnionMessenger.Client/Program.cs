using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client;
using BlazorWASMOnionMessenger.Client.AuthProviders;
using BlazorWASMOnionMessenger.Client.Features.Auth;
using BlazorWASMOnionMessenger.Client.Features.Chats;
using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.Features.Messages;
using BlazorWASMOnionMessenger.Client.Features.Participants;
using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Client.WebRtc;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSingleton<JwtTokenParser>();
builder.Services.AddScoped<IWebRtcService, WebRtcService>( provider =>
{
    var jsRuntime = provider.GetRequiredService<IJSRuntime>();
    var dialogService = provider.GetRequiredService<DialogService>();
    var webRtcHubUrl = builder.Configuration["webRtcHubUrl"];
    return new WebRtcService(jsRuntime, dialogService, webRtcHubUrl);
});
builder.Services.AddScoped<ISignalRMessageService, SignalRMessageService>( provider =>
{
    var hubUrl = builder.Configuration["messageHubUrl"];
    return new SignalRMessageService(hubUrl);
});
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<DialogService>();

await builder.Build().RunAsync();
