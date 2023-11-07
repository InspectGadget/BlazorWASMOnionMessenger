using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Reflection;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class Grid : ComponentBase
    {
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public int Page { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public int PageSize { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string OrderBy { get; set; } = String.Empty;
        [SupplyParameterFromQuery]
        [Parameter]
        public bool OrderType { get; set; } = false;
        [SupplyParameterFromQuery]
        [Parameter]
        public string? Search { get; set; } = String.Empty;

        protected List<UserDto> users = new List<UserDto>();
        protected int TotalUsers { get; set; }
        protected List<string> orderProps = new List<string>();
        protected List<int> pageSizeOptions = new List<int> { 2, 5, 10, 20 };

        protected override async Task OnInitializedAsync()
        {
            if (Page == 0) Page = 1;
            if (PageSize == 0) PageSize = 2;
            PopulateOrderProps();
            if (OrderBy == string.Empty) OrderBy = orderProps[0];
            await Fetch();
        }

        protected async void OnPageChange(PagerEventArgs args)
        {
            Page = args.PageIndex + 1;
            await Navigate();
        }

        protected async void OnCheckBoxChange(bool value) => await Navigate();

        protected async Task Navigate()
        {
            NavigationManager.NavigateTo($"/users?page={Page}&pageSize={PageSize}&orderBy={OrderBy}&orderType={OrderType}&search={Search}");
            await Fetch();
        }

        private async Task Fetch()
        {
            var result = await UserService.GetPage(Page, PageSize, OrderBy, OrderType, Search);
            if (result.IsSuccessful)
            {
                users = result.Entities;
                TotalUsers = result.Quantity;
            }
            StateHasChanged();
        }

        private void PopulateOrderProps()
        {
            PropertyInfo[] properties = typeof(UserDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                orderProps.Add(property.Name);
            }
        }
    }
}
