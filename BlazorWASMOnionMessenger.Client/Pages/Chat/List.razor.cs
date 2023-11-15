using BlazorWASMOnionMessenger.Client.Features.Chats;
using BlazorWASMOnionMessenger.Client.Shared;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.Pages.Chat
{
    public partial class List : BaseGrid<ChatDto>
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IChatService ChatService { get; set; }

        private const string Route = "/chats";

        private const int GroupChatTypeId = 2;

        protected CreateChatDto CreateChatDto { get; set; } = new CreateChatDto();


        protected override async Task OnInitializedAsync()
        {
            if (Page == 0) Page = 1;
            if (PageSize == 0) PageSize = 2;
            PopulateOrderProps();
            if (string.IsNullOrEmpty(OrderBy)) OrderBy = OrderProps[0];
            await Fetch();
        }

        protected override async Task Navigate()
        {
            NavigationManager.NavigateTo(ConstructRouteTemplate(Route));
            await Fetch();
        }
        protected void OnRowClick(DataGridRowMouseEventArgs<ChatDto> row)
        {
            NavigationManager.NavigateTo($"/chat/{row.Data.Id}");
        }

        private async Task Fetch()
        {
            isLoading = true;
            var result = await ChatService.GetPage(Page, PageSize, OrderBy, OrderType, Search);
            if (result.IsSuccessful)
            {
                Items = result.Entities;
                Total = result.Quantity;
            }
            
            isLoading = false;
            StateHasChanged();
        }

        protected async Task CreateGroup()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            var userId = user.FindFirst("nameid").Value;
            CreateChatDto.CreatorId = userId;
            CreateChatDto.ChatTypeId = GroupChatTypeId;
            var chatId = await ChatService.CreateChat(CreateChatDto);
            NavigationManager.NavigateTo($"/chat/{chatId}");
        }
    }
}
