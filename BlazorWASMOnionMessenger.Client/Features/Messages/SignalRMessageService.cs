﻿using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public class SignalRMessageService : ISignalRMessageService
    {
        private readonly string hubUrl;
        private HubConnection hubConnection;

        public event Action<MessageDto> OnReceiveMessage;
        public event Action<MessageDto> OnUpdateMessage;
        public event Action<MessageDto> OnDeleteMessage;

        public event Action<int, string, string> OnSignalWebRtc;

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
                OnReceiveMessage?.Invoke(message);
            });
            hubConnection.On<MessageDto>("UpdateMessage", message =>
            {
                OnUpdateMessage?.Invoke(message);
            });
            hubConnection.On<MessageDto>("DeleteMessage", message =>
            {
                OnDeleteMessage?.Invoke(message);
            });
            hubConnection.On<int, string, string>("SignalWebRtc", (chatId, type, payload) => {
                OnSignalWebRtc?.Invoke(chatId, type, payload);
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

        public async Task SendMessageToChat(CreateMessageDto newMessageDto)
        {
            await hubConnection.SendAsync("SendMessageToChat", newMessageDto);
        }
        public async Task UpdateMessageInChat(MessageDto messageDto)
        {
            await hubConnection.SendAsync("UpdateMessageInChat", messageDto);
        }

        public async Task DeleteMessageFromChat(MessageDto messageDto)
        {
            await hubConnection.SendAsync("DeleteMessageFromChat", messageDto);
        }

        public void SubscribeToReceiveMessage(Action<MessageDto> handler)
        {
            OnReceiveMessage += handler;
        }

        public void UnsubscribeFromReceiveMessage(Action<MessageDto> handler)
        {
            OnReceiveMessage -= handler;
        }

        public void SubscribeToDeleteMessage(Action<MessageDto> handler)
        {
            OnDeleteMessage += handler;
        }

        public void UnsubscribeFromDeleteMessage(Action<MessageDto> handler)
        {
            OnDeleteMessage -= handler;
        }

        public void SubscribeToUpdateMessage(Action<MessageDto> handler)
        {
            OnUpdateMessage += handler;
        }

        public void UnsubscribeFromUpdateMessage(Action<MessageDto> handler)
        {
            OnUpdateMessage -= handler;
        }
        public void SubscribeToSignalWebRtc(Action<int, string, string> handler)
        {
            OnSignalWebRtc += handler;
        }

        public void UnsubscribeFromSignalWebRtc(Action<int, string, string> handler)
        {
            OnSignalWebRtc -= handler;
        }

        public HubConnection GetHub()
        {
            return this.hubConnection;
        }
    }
}
