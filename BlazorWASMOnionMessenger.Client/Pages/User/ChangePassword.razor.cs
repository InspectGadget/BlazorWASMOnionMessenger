using BlazorWASMOnionMessenger.Client.Features.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.Pages.User
{
    public partial class ChangePassword
    {
        protected UserChangePasswordDto userChangePasswordDto = new UserChangePasswordDto();
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected NotificationMessage NotificationMessage { get; set; } = new NotificationMessage()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Successfull",
            Detail = "Password updated",
            Duration = 4000
        };
        public async Task HandleChangePassword()
        {
            var response = await UserService.ChangePassword(userChangePasswordDto);

            if (response.IsSuccessful)
            {
                NotificationService.Notify(NotificationMessage);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", response.ErrorMessage, 4000);
            }
        }
    }
}
