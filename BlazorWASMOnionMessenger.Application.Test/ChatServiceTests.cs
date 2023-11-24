using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.MappingProfiles;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.Entities;
using MockQueryable.Moq;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task CreateChat_PrivateChatExist_ReturnsChatId()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
            var mapperMock = new Mock<IMapper>();
            var participantServiceMock = new Mock<IParticipantService>();

            var chatService = new ChatService(unitOfWorkMock.Object, searchPredicateBuilderMock.Object, mapperMock.Object, participantServiceMock.Object);

            var createChatDto = new CreateChatDto
            {
                ChatTypeId = 1
            };

            var chatRepositoryMock = new Mock<IGenericRepository<Chat>>();
            unitOfWorkMock.Setup(u => u.Repository<Chat>()).Returns(chatRepositoryMock.Object);


            // Assume there's an existing chat
            var chatQuery = new List<Chat>() { new() { Id = 1 } }.BuildMock();
            chatRepositoryMock.Setup(x => x.GetQueryable(
            It.IsAny<Expression<Func<Chat, bool>>>(),
            It.IsAny<Func<IQueryable<Chat>, IOrderedQueryable<Chat>>>()))
            .Returns(chatQuery);

            // Act
            var result = await chatService.CreateChat(createChatDto);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task CreateChat_Private_ReturnsChatId()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
            var mapperMock = new Mock<IMapper>();
            var participantServiceMock = new Mock<IParticipantService>();

            var chatService = new ChatService(unitOfWorkMock.Object, searchPredicateBuilderMock.Object, mapperMock.Object, participantServiceMock.Object);

            var createChatDto = new CreateChatDto
            {
                ChatTypeId = 1,
            };

            var chatRepositoryMock = new Mock<IGenericRepository<Chat>>();
            unitOfWorkMock.Setup(u => u.Repository<Chat>()).Returns(chatRepositoryMock.Object);

            mapperMock.Setup(m => m.Map<Chat>(It.IsAny<CreateChatDto>()))
                      .Returns(new Chat { Id = 1 });

            var chatQuery = new List<Chat>().BuildMock();
            // Assume there's no existing chat with the same participants
            chatRepositoryMock.Setup(x => x.GetQueryable(
                It.IsAny<Expression<Func<Chat, bool>>>(),
                It.IsAny<Func<IQueryable<Chat>, IOrderedQueryable<Chat>>>()))
                .Returns(chatQuery);

            // Act
            var result = await chatService.CreateChat(createChatDto);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetChatById_ExistingChat_ReturnsChatDto()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
            var mapperMock = new Mock<IMapper>();
            var participantServiceMock = new Mock<IParticipantService>();


            var chatService = new ChatService(unitOfWorkMock.Object, searchPredicateBuilderMock.Object, mapperMock.Object, participantServiceMock.Object);

            var chatRepositoryMock = new Mock<IGenericRepository<Chat>>();
            unitOfWorkMock.Setup(u => u.Repository<Chat>()).Returns(chatRepositoryMock.Object);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new ChatMappingProfile()); }));

            var chatQuery = GetTestChat().BuildMock();
            chatRepositoryMock.Setup(x => x.GetQueryable(
            It.IsAny<Expression<Func<Chat, bool>>>(),
            It.IsAny<Func<IQueryable<Chat>, IOrderedQueryable<Chat>>>()))
            .Returns(chatQuery);

            // Act
            var result = await chatService.GetChatById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetChatById_NonExistingChat_ReturnsNull()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
            var mapperMock = new Mock<IMapper>();
            var participantServiceMock = new Mock<IParticipantService>();

            var chatService = new ChatService(unitOfWorkMock.Object, searchPredicateBuilderMock.Object, mapperMock.Object, participantServiceMock.Object);

            var chatRepositoryMock = new Mock<IGenericRepository<Chat>>();
            unitOfWorkMock.Setup(u => u.Repository<Chat>()).Returns(chatRepositoryMock.Object);

            var chatQuery = new List<Chat>().BuildMock();
            chatRepositoryMock.Setup(x => x.GetQueryable(
            It.IsAny<Expression<Func<Chat, bool>>>(),
            It.IsAny<Func<IQueryable<Chat>, IOrderedQueryable<Chat>>>()))
            .Returns(chatQuery);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new ChatMappingProfile()); }));

            // Act
            var result = await chatService.GetChatById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetChatsPage_InvalidPageOrPageSize_ThrowsValidationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var searchPredicateBuilderMock = new Mock<ISearchPredicateBuilder>();
            var mapperMock = new Mock<IMapper>();
            var participantServiceMock = new Mock<IParticipantService>();

            var chatService = new ChatService(unitOfWorkMock.Object, searchPredicateBuilderMock.Object, mapperMock.Object, participantServiceMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.GetChatsPage(0, 10, "orderBy", true, "search"));
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.GetChatsPage(1, 0, "orderBy", true, "search"));
        }
        private List<Chat> GetTestChat()
        {
            var messages = new List<Message>{ new Message
            {
                Id = 1,
                MessageText = "Test",
                CreatedAt = DateTime.UtcNow,
                ApplicationUser = new ApplicationUser
                {
                    Id = "",
                    UserName = "Test",
                }
            }};

            var chat = new Chat() { 
                Id = 1, 
                ChatTypeId = 1, 
                CreatedAt = DateTime.Now, 
                ChatType = new ChatType
                {
                    Id = 1,
                    Name = "Test"
                }, 
                Messages = messages };
            var testChat = new List<Chat>() { chat };
            return testChat;
        }
    }
}
