using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Domain.Entities;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.UserName)
                ?? throw new CustomAuthenticationException("User not found.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new CustomAuthenticationException("Invalid password.");
            }

            return _tokenService.CreateToken(user);
        }

        public async Task<string> Register(UserRegisterDto userRegisterDto)
        {
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.UserName) != null;

            if (userExists)
            {
                throw new CustomAuthenticationException("User with the same username already exists.");
            }

            var user = _mapper.Map<ApplicationUser>(userRegisterDto);

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                throw new CustomAuthenticationException("Registration failed.");
            }

            return _tokenService.CreateToken(user);
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
                users = users.Where(BuildSearchExpression(search));
            }

            var quantity = users.Count();

            if (!string.IsNullOrEmpty(orderBy)) users = users.OrderBy(orderBy + (orderType ? " desc" : " asc"));

            var pageUsers = await users.Skip((page - 1) * pageSize).Take(pageSize)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();

            return new PagedEntities<UserDto>(pageUsers)
            {
                Quantity = quantity,
                Pages = (int)Math.Ceiling((double)quantity / pageSize)
            };
        }

        private static Expression<Func<ApplicationUser, bool>> BuildSearchExpression(string search)
        {
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var predicate = PredicateBuilder.New<ApplicationUser>(false);

            foreach (var keyword in keywords)
            {
                predicate = predicate.Or(user =>
                    user.UserName.Contains(keyword) ||
                    user.PhoneNumber.Contains(keyword) ||
                    user.FirstName.Contains(keyword) ||
                    user.LastName.Contains(keyword) 
                );
            }

            return predicate;
        }
    }
}
