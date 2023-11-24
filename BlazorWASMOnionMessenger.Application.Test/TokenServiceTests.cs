using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class TokenServiceTests
    {
        [Fact]
        public void CreateToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "1",
                UserName = "testuser"
            };

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["JWT:Secret"]).Returns("yJWTAuthenticationDevPurposePassword");

            var tokenService = new TokenService(configMock.Object);

            // Act
            var token = tokenService.CreateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.True(!string.IsNullOrWhiteSpace(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            Assert.NotNull(jwtToken);
            Assert.Equal("testuser", jwtToken?.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value);
            Assert.Equal("1", jwtToken?.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value);
        }

        [Fact]
        public void CreateToken_NullUser_ThrowsException()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["JWT:Secret"]).Returns("yJWTAuthenticationDevPurposePassword");

            var tokenService = new TokenService(configMock.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tokenService.CreateToken(null));
        }
    }
}
