using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Common.MappingProfiles;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.DTOs.Participant;
using BlazorWASMOnionMessenger.Domain.Entities;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class ParticipantServiceTests
    {
        [Fact]
        public async Task AddParticipantToChat_ValidParticipant_SaveChanges()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new ParticipantService(unitOfWorkMock.Object, mapperMock.Object);

            var createParticipantDto = new CreateParticipantDto();
            var mappedParticipant = new Participant();

            unitOfWorkMock.Setup(u => u.Repository<Participant>().Add(It.IsAny<Participant>()));
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);
            mapperMock.Setup(m => m.Map<Participant>(createParticipantDto)).Returns(mappedParticipant);

            // Act
            await service.AddParticipantToChat(createParticipantDto);

            // Assert
            unitOfWorkMock.Verify(u => u.Repository<Participant>().Add(mappedParticipant), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task AddParticipantToChat_ExceptionOnSave_ThrowsServiceException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new ParticipantService(unitOfWorkMock.Object, mapperMock.Object);

            var createParticipantDto = new CreateParticipantDto();
            var mappedParticipant = new Participant();

            unitOfWorkMock.Setup(u => u.Repository<Participant>().Add(It.IsAny<Participant>()));
            unitOfWorkMock.Setup(u => u.SaveAsync()).ThrowsAsync(new RepositoryException());

            mapperMock.Setup(m => m.Map<Participant>(createParticipantDto)).Returns(mappedParticipant);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => service.AddParticipantToChat(createParticipantDto));
        }

        [Fact]
        public async Task GetByChatIdAsync_ValidChatId_ReturnsParticipants()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new ParticipantService(unitOfWorkMock.Object, mapperMock.Object);

            var chatId = 1;
            var participants = new List<Participant> { new Participant { 
                Id = 1, 
                ChatId = 1, 
                ApplicationUser = new ApplicationUser { 
                    UserName = "Test" } 
            } }.BuildMock();

            unitOfWorkMock.Setup(u => u.Repository<Participant>().GetQueryable(
                It.IsAny<Expression<Func<Participant, bool>>>(),
                It.IsAny<Func<IQueryable<Participant>, IOrderedQueryable<Participant>>>()))
                .Returns(participants);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new ParticipantMappingProfile()); }));

            var expected = new List<ParticipantDto> { new ParticipantDto
            {
                Id = 1,
                ChatId = 1,
                UserName = "Test",
            } };

            // Act
            var result = await service.GetByChatIdAsync(chatId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public async Task GetByChatIdAsync_ExceptionOnQuery_ThrowsServiceException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new ParticipantService(unitOfWorkMock.Object, mapperMock.Object);

            var chatId = 1;

            unitOfWorkMock.Setup(u => u.Repository<Participant>().GetQueryable(
                It.IsAny<Expression<Func<Participant, bool>>>(),
                It.IsAny<Func<IQueryable<Participant>, IOrderedQueryable<Participant>>>()))
                .Throws(new RepositoryException());

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => service.GetByChatIdAsync(chatId));
        }
    }
}
