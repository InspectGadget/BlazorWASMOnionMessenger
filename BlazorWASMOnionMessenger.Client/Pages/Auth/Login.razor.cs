using BlazorWASMOnionMessenger.Client.Features.Auth;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages.Auth
{
    public partial class Login
    {
        private LoginDto _userLoginDto = new LoginDto();
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; } = string.Empty;
        public async Task HandleLogin()
        {
            ShowAuthError = false;
            var result = await AuthService.Login(_userLoginDto);
            if (!result.IsSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
