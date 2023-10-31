using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages
{
    public partial class ChangePassword
    {
        private UserChangePasswordDto _userChangePasswordDto = new UserChangePasswordDto();
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }
        public async Task HandleChangePassword()
        {
            ShowAuthError = false;
            var result = await UserService.ChangePassword(_userChangePasswordDto);
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
