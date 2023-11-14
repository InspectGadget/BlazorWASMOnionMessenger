using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using System.Web;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Client.Features.Helpers;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService httpClientService;

        public UserService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<AuthDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var response = await httpClientService.PostAsync<UserChangePasswordDto, AuthDto>("user/changepassword", userChangePasswordDto);
            if (!response.IsSuccessful) return response;
            return new AuthDto { IsSuccessful = true };
        }

        public async Task<UserDto> GetById(string userId)
        {
            return await httpClientService.GetAsync<UserDto>("user/" + userId);
        }

        public async Task<ResponseDto> Update(UserDto userDto)
        {
            return await httpClientService.PutAsync("user/" + userDto.Id, userDto);
        }

        public async Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            string queryString = QueryStringGenerator.GenerateGridQueryString(page, pageSize, orderBy, orderType, search);
            return await httpClientService.GetAsync<PagedEntities<UserDto>>($"user/page?{queryString}");
        }

        
    }
}
