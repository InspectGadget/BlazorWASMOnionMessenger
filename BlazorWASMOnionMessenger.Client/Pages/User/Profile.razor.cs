using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class Profile : ComponentBase
    {
        private UserDto _userDto = new UserDto();
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected NotificationMessage NotificationMessage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Successfull",
            Detail = "Profile updated",
            Duration = 4000
        };
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
            var response = await UserService.Update(_userDto);
            if (response.IsSuccessful)
            {
                NotificationService.Notify(NotificationMessage);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", response.ErrorMessage, 4000);
            }
        }
    }
}
