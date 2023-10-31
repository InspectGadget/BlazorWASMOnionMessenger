using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMOnionMessenger.Client.Pages
{
    public partial class Register
    {
        private UserRegisterDto _userRegisterDto = new UserRegisterDto();
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }
        public async Task HandleRegister()
        {
            ShowAuthError = false;
            var result = await UserService.Register(_userRegisterDto);
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
