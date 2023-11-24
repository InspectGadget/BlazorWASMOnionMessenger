using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.Entities;
using Moq;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class ChatTypeServiceTests
    {
        [Fact]
        public async Task GetChatTypesAsync_Success()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<ChatType>>();

            unitOfWorkMock.Setup(u => u.Repository<ChatType>()).Returns(repositoryMock.Object);

            var chatTypes = new List<ChatType>();
            repositoryMock.Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<ChatType, bool>>>(),
                    It.IsAny<Func<IQueryable<ChatType>, IOrderedQueryable<ChatType>>>()))
                .ReturnsAsync(chatTypes);

            var chatTypeService = new ChatTypeService(unitOfWorkMock.Object);

            // Act
            var result = await chatTypeService.GetChatTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(chatTypes, result);
        }

        [Fact]
        public async Task GetChatTypesAsync_RepositoryException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<ChatType>>();

            unitOfWorkMock.Setup(u => u.Repository<ChatType>()).Returns(repositoryMock.Object);

            repositoryMock.Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<ChatType, bool>>>(),
                    It.IsAny<Func<IQueryable<ChatType>, IOrderedQueryable<ChatType>>>()))
                .ThrowsAsync(new RepositoryException("Simulated repository exception"));

            var chatTypeService = new ChatTypeService(unitOfWorkMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ServiceException>(() => chatTypeService.GetChatTypesAsync());
        }
    }
}
