using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces;
using BlazorWASMOnionMessenger.Application.Interfaces.Users;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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
                throw new CustomAuthenticationException("User with the same email or username already exists.");
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
    }
}
