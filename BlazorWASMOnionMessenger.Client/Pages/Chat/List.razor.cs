using BlazorWASMOnionMessenger.Client.Features.Chats;
using BlazorWASMOnionMessenger.Client.Shared;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.Pages.Chat
{
    public partial class List : BaseGrid<ChatDto>
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IChatService ChatService { get; set; }

        private const string Route = "/chats";


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
    }
}
