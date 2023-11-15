using BlazorWASMOnionMessenger.Client.Features.Chats;
using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Client.Shared;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class Grid : BaseGrid<UserDto>
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IChatService ChatService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        private const int PrivateChatId = 1;

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
            NavigationManager.NavigateTo($"/users?page={Page}&pageSize={PageSize}&orderBy={OrderBy}&orderType={OrderType}&search={Search}");
            await Fetch();
        }

        private async Task Fetch()
        {
            isLoading = true;
            var result = await UserService.GetPage(Page, PageSize, OrderBy, OrderType, Search);
            if (result.IsSuccessful)
            {
                Items = result.Entities;
                Total = result.Quantity;
            }
            isLoading = false;
            StateHasChanged();
        }
        protected async Task OnRowClick(DataGridRowMouseEventArgs<UserDto> row)
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            var userId = user.FindFirst("nameid").Value;
            var chatId = await ChatService.CreateChat(new Domain.DTOs.Chat.CreateChatDto
            {
                ChatTypeId = PrivateChatId,
                CreatorId = userId,
                ParticipantId = row.Data.Id
            });
            NavigationManager.NavigateTo($"/chat/{chatId}");
        }
    }
}
