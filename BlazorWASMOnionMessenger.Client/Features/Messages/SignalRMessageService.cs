using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public class SignalRMessageService : ISignalRMessageService
    {
        private readonly string hubUrl;
        private HubConnection hubConnection;

        public event Action<MessageDto> OnMessageReceived;

        public SignalRMessageService(string hubUrl)
        {
            this.hubUrl = hubUrl;
        }
        public void CreateAsync(string token)
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();

            RegisterHubEvents();
        }
        private void RegisterHubEvents()
        {
            hubConnection.On<MessageDto>("ReceiveMessage", message =>
            {
                OnMessageReceived?.Invoke(message);
            });
        }

        public async Task StartConnection()
        {
            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("SignalR connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connecting to SignalR hub: {ex.Message}");
            }
        }

        public async Task SendMessageToChat(NewMessageDto newMessageDto)
        {
            await hubConnection.SendAsync("SendMessageToChat", newMessageDto);
        }
        public void SubscribeToMessageReceived(Action<MessageDto> handler)
        {
            OnMessageReceived += handler;
        }

        public void UnsubscribeFromMessageReceived(Action<MessageDto> handler)
        {
            OnMessageReceived -= handler;
        }
    }
}
