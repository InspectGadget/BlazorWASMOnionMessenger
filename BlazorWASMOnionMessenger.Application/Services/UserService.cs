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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ISearchPredicateBuilder searchPredicateBuilder;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, ISearchPredicateBuilder searchPredicateBuilder)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.searchPredicateBuilder = searchPredicateBuilder;
        }

        public async Task ChangePassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId)
                    ?? throw new CustomAuthenticationException("User not found.");

                var changePasswordResult = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (!changePasswordResult.Succeeded)
                {
                    throw new CustomAuthenticationException("Password change failed.");
                }
            }
            catch (Exception ex) when (ex is CustomAuthenticationException || ex is InvalidOperationException)
            {
                throw; // Re-throw CustomAuthenticationException directly
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred while changing password.", ex);
            }
        }

        public async Task UpdateUser(UserDto userDto, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if(user == null) throw new ServiceException("User not found.");

                user.UserName = userDto.UserName;
                user.PhoneNumber = userDto.PhoneNumber;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new RepositoryException("Failed to update user.");
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred while updating user.", ex);
            }
        }

        public async Task<UserDto> GetById(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userId} not found.");
                }

                return mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred while getting user by ID.", ex);
            }
        }

        public async Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            try
            {
                if (page <= 0)
                {
                    throw new ArgumentException("Page number must be greater than 0.", nameof(page));
                }

                if (pageSize <= 0)
                {
                    throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));
                }

                var users = userManager.Users.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(searchPredicateBuilder.
                        BuildSearchPredicate<ApplicationUser, UserDto>(search));
                }

                var quantity = users.Count();

                if (!string.IsNullOrEmpty(orderBy)) users = users.OrderBy(orderBy + " " + (orderType ? "desc" : "asc"));

                var pageUsers = await users.Skip((page - 1) * pageSize).Take(pageSize)
                    .ProjectTo<UserDto>(mapper.ConfigurationProvider).ToListAsync();

                return new PagedEntities<UserDto>(pageUsers)
                {
                    Quantity = quantity,
                    Pages = (int)Math.Ceiling((double)quantity / pageSize)
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred while retrieving user page.", ex);
            }
        }
    }
}
