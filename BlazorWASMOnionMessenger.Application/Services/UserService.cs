using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ISearchPredicateBuilder _searchPredicateBuilder;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, ISearchPredicateBuilder searchPredicateBuilder)
        {
            _userManager = userManager;
            _mapper = mapper;
            _searchPredicateBuilder = searchPredicateBuilder;
        }

        public async Task ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new CustomAuthenticationException("User not found.");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                throw new CustomAuthenticationException("Password change failed.");
            }
        }

        public async Task UpdateUser(UserDto userDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new RepositoryException("User not found.");

            user.UserName = userDto.UserName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new RepositoryException("Failed to update.");
            }
        }

        public async Task<UserDto> GetById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(_searchPredicateBuilder.
                    BuildSearchPredicate<ApplicationUser, UserDto>(search));
            }

            var quantity = users.Count();

            if (!string.IsNullOrEmpty(orderBy)) users = users.OrderBy(orderBy + " " + (orderType ? "desc" : "asc"));

            var pageUsers = await users.Skip((page - 1) * pageSize).Take(pageSize)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();

            return new PagedEntities<UserDto>(pageUsers)
            {
                Quantity = quantity,
                Pages = (int)Math.Ceiling((double)quantity / pageSize)
            };
        }
    }
}
