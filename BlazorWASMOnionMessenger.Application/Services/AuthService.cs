using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces;
using BlazorWASMOnionMessenger.Application.Interfaces.Authentication;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }
        public async Task<string> Login(LoginDto userLoginDto)
        {
            var user = await userManager.FindByNameAsync(userLoginDto.UserName)
                ?? throw new CustomAuthenticationException("User not found.");

            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new CustomAuthenticationException("Invalid password.");
            }

            return tokenService.CreateToken(user);
        }

        public async Task<string> Register(RegisterDto userRegisterDto)
        {
            var userExists = await userManager.FindByNameAsync(userRegisterDto.UserName) != null;

            if (userExists)
            {
                throw new CustomAuthenticationException("User with the same username already exists.");
            }

            var user = mapper.Map<ApplicationUser>(userRegisterDto);

            var result = await userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                throw new CustomAuthenticationException("Registration failed.");
            }

            return tokenService.CreateToken(user);
        }
    }
}
