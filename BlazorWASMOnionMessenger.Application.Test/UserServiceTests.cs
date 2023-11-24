using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Common.MappingProfiles;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ISearchPredicateBuilder> searchPredicateBuilderMock;


        public UserServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mapperMock = new Mock<IMapper>();
            searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
        }
        [Fact]
        public async Task ChangePassword_ValidParameters_PasswordChangedSuccessfully()
        {
            // Arrange
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            userManagerMock.Setup(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object, Mock.Of<IMapper>(), Mock.Of<ISearchPredicateBuilder>());

            // Act
            await userService.ChangePassword("userId", "currentPassword", "newPassword");

            // Assert
            userManagerMock.Verify(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_UserNotFound_ThrowsCustomAuthenticationException()
        {
            // Arrange
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            var userService = new UserService(userManagerMock.Object, Mock.Of<IMapper>(), Mock.Of<ISearchPredicateBuilder>());

            // Act & Assert
            await Assert.ThrowsAsync<CustomAuthenticationException>(() =>
                userService.ChangePassword("userId", "currentPassword", "newPassword"));
        }

        [Fact]
        public async Task UpdateUser_ValidUserDto_SuccessfullyUpdatesUser()
        {
            // Arrange
            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            var userId = "1";
            var userDto = new UserDto
            {
                UserName = "newUsername",
                PhoneNumber = "123456789",
                FirstName = "John",
                LastName = "Doe"
            };

            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            userManagerMock.Setup(u => u.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await userService.UpdateUser(userDto, userId);

            // Assert
            userManagerMock.Verify(
                m => m.FindByIdAsync(userId),
                Times.Once);

            userManagerMock.Verify(
                m => m.UpdateAsync(It.IsAny<ApplicationUser>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound_ThrowsServiceException()
        {
            // Arrange
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            var userId = "1";
            var userDto = new UserDto();

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => userService.UpdateUser(userDto, userId));

            userManagerMock.Verify(
                m => m.FindByIdAsync(userId),
                Times.Once);

            userManagerMock.Verify(
                m => m.UpdateAsync(It.IsAny<ApplicationUser>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateUser_FailedToUpdateUser_ThrowsServiceException()
        {
            // Arrange
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed." }));

            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            var userId = "1";
            var userDto = new UserDto();

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => userService.UpdateUser(userDto, userId));

            userManagerMock.Verify(
                m => m.FindByIdAsync(userId),
                Times.Once);

            userManagerMock.Verify(
                m => m.UpdateAsync(It.IsAny<ApplicationUser>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingUserId_ReturnsUserDto()
        {
            // Arrange

            var userId = "1";
            var existingUser = new ApplicationUser { Id = userId, UserName = "ExistingUser" };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(existingUser);
            mapperMock.Setup(m => m.Map<UserDto>(existingUser)).Returns(new UserDto { Id = userId, UserName = "ExistingUser" });

            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            // Act
            var result = await userService.GetById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("ExistingUser", result.UserName);

            userManagerMock.Verify(
                m => m.FindByIdAsync(userId),
                Times.Once);

            mapperMock.Verify(
                m => m.Map<UserDto>(existingUser),
                Times.Once);
        }

        [Fact]
        public async Task GetById_NonExistingUserId_ThrowsServiceException()
        {
            // Arrange
            var userId = "2";
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);
            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ServiceException>(() => userService.GetById(userId));

            userManagerMock.Verify(
                m => m.FindByIdAsync(userId),
                Times.Once);
        }

        [Fact]
        public async Task GetPage_ValidArguments_ReturnsPagedEntities()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var orderBy = "UserName";
            var orderType = false;
            var search = "John";

            var users = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = "1", UserName = "JohnDoe" },
                    new ApplicationUser { Id = "2", UserName = "JohnSmith" }
                }.BuildMock();

            userManagerMock.Setup(u => u.Users).Returns(users);
            searchPredicateBuilderMock.Setup(s => s.BuildSearchPredicate<ApplicationUser, UserDto>(search))
                .Returns(u => u.UserName.Contains(search));

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new UserMappingProfile()); }));

            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            // Act
            var result = await userService.GetPage(page, pageSize, orderBy, orderType, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(1, result.Pages);
            Assert.Collection(result.Entities,
                user => Assert.Equal("JohnDoe", user.UserName),
                user => Assert.Equal("JohnSmith", user.UserName));

            userManagerMock.Verify(
                u => u.Users,
                Times.Once);

            searchPredicateBuilderMock.Verify(
                s => s.BuildSearchPredicate<ApplicationUser, UserDto>(search),
                Times.Once);
        }

        [Fact]
        public async Task GetPage_InvalidArguments_ThrowsArgumentException()
        {
            // Arrange
            var userService = new UserService(userManagerMock.Object, mapperMock.Object, searchPredicateBuilderMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ServiceException>(() => userService.GetPage(0, 10, "UserName", true, "John"));
            await Assert.ThrowsAsync<ServiceException>(() => userService.GetPage(1, 0, "UserName", true, "John"));
        }
    }
}
