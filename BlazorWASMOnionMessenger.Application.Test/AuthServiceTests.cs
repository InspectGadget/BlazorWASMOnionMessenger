using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> signInManagerMock;
        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                    userManagerMock.Object,
                    Mock.Of<IHttpContextAccessor>(),
                    Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
        }
        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            var mapperMock = new Mock<IMapper>();

            var authService = new AuthService(userManagerMock.Object, signInManagerMock.Object, tokenServiceMock.Object, mapperMock.Object);

            var loginDto = new LoginDto
            {
                UserName = "testuser",
                Password = "password123"
            };

            var user = new ApplicationUser { UserName = "testuser" };
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            tokenServiceMock.Setup(m => m.CreateToken(It.IsAny<ApplicationUser>())).Returns("testtoken");

            // Act
            var result = await authService.Login(loginDto);

            // Assert
            Assert.Equal("testtoken", result);
        }

        [Fact]
        public async Task Login_UserNotFound_ThrowsCustomAuthenticationException()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            var mapperMock = new Mock<IMapper>();

            var authService = new AuthService(userManagerMock.Object, signInManagerMock.Object, tokenServiceMock.Object, mapperMock.Object);

            var loginDto = new LoginDto
            {
                UserName = "nonexistentuser",
                Password = "password123"
            };

            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act and Assert
            await Assert.ThrowsAsync<CustomAuthenticationException>(() => authService.Login(loginDto));
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsCustomAuthenticationException()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            var mapperMock = new Mock<IMapper>();

            var authService = new AuthService(userManagerMock.Object, signInManagerMock.Object, tokenServiceMock.Object, mapperMock.Object);

            var loginDto = new LoginDto
            {
                UserName = "testuser",
                Password = "invalidpassword"
            };

            var user = new ApplicationUser { UserName = "testuser" };
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Act and Assert
            await Assert.ThrowsAsync<CustomAuthenticationException>(() => authService.Login(loginDto));
        }

        [Fact]
        public async Task Register_ValidRegistration_ReturnsToken()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            var mapperMock = new Mock<IMapper>();

            var authService = new AuthService(userManagerMock.Object, signInManagerMock.Object, tokenServiceMock.Object, mapperMock.Object);

            var registerDto = new RegisterDto
            {
                UserName = "newuser",
                Password = "newpassword"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            tokenServiceMock.Setup(m => m.CreateToken(It.IsAny<ApplicationUser>())).Returns("testtoken");

            // Act
            var result = await authService.Register(registerDto);

            // Assert
            Assert.Equal("testtoken", result);
        }

        [Fact]
        public async Task Register_UserExists_ThrowsCustomAuthenticationException()
        {
            // Arrange
            var tokenServiceMock = new Mock<ITokenService>();
            var mapperMock = new Mock<IMapper>();

            var authService = new AuthService(userManagerMock.Object, signInManagerMock.Object, tokenServiceMock.Object, mapperMock.Object);

            var registerDto = new RegisterDto
            {
                UserName = "existinguser",
                Password = "newpassword"
            };

            var existingUser = new ApplicationUser { UserName = "existinguser" };
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsAsync<CustomAuthenticationException>(() => authService.Register(registerDto));
        }
    }
}