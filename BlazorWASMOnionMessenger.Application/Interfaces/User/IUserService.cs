using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task ChangePassword(string userId, string currentPassword, string newPassword);
        Task<UserDto> GetById(string userId);
        Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search);
        Task UpdateUser(UserDto userDto, string userId);
    }
}
