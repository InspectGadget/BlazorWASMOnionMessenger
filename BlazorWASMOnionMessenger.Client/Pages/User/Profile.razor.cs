using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class Profile : ComponentBase
    {
        private UserDto _userDto = new UserDto();
        [Inject]
        private IUserService UserService { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            var userIdClaim = user.FindFirst("nameid");
            _userDto = await UserService.GetById(userIdClaim.Value);
        }
        private async Task UpdateProfile()
        {
            await UserService.Update(_userDto);
        }
    }
}
