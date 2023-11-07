using BlazorWASMOnionMessenger.Client.Features.Auth;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages.Auth
{
    public partial class Logout
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
