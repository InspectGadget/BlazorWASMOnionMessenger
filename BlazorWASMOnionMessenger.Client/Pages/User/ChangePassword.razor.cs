using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class ChangePassword
    {
        protected UserChangePasswordDto _userChangePasswordDto = new UserChangePasswordDto();
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        protected bool ShowAuthError { get; set; }
        protected string Error { get; set; }
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
