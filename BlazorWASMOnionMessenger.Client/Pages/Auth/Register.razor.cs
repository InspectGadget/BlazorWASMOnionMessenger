using BlazorWASMOnionMessenger.Client.Features.Auth;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages.Auth
{
    public partial class Register
    {
        private RegisterDto _userRegisterDto = new RegisterDto();
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }
        public async Task HandleRegister()
        {
            ShowAuthError = false;
            var result = await AuthService.Register(_userRegisterDto);
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
