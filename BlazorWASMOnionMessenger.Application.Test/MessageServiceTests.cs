using AutoMapper;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Common.MappingProfiles;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Application.Services;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using BlazorWASMOnionMessenger.Domain.Entities;
using MockQueryable.Moq;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Test
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task CreateMessageAsync_ValidMessage_ReturnsMessageId()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var newMessageDto = new CreateMessageDto();

            unitOfWorkMock.Setup(u => u.Repository<Message>().Add(It.IsAny<Message>()));
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            mapperMock.Setup(m => m.Map<Message>(It.IsAny<CreateMessageDto>())).Returns(new Message());

            // Act
            await service.CreateMessageAsync(newMessageDto);

            // Assert
            unitOfWorkMock.Verify(u => u.Repository<Message>().Add(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetMessagesAsync_ValidParametersWithoutUnreadMessages_ReturnsMessageDtos()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var userId = "testUserId";
            var chatId = 1;
            var quantity = 10;
            var skip = 0;

            var messages = GetTestMessages().BuildMock();
            var unreadMessages = new List<UnreadMessage>().BuildMock();


            unitOfWorkMock.Setup(u => u.Repository<Message>().GetQueryable(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IOrderedQueryable<Message>>>()))
                .Returns(messages);

            unitOfWorkMock.Setup(u => u.Repository<UnreadMessage>().GetQueryable(
                It.IsAny<Expression<Func<UnreadMessage, bool>>>(),
                It.IsAny<Func<IQueryable<UnreadMessage>, IOrderedQueryable<UnreadMessage>>>()))
                .Returns(unreadMessages);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new MessageMappingProfile()); }));

            var expected = GetTestMessageDtos();

            // Act
            var result = await service.GetMessagesAsync(userId, chatId, quantity, skip);

            // Assert
            Assert.Equal(expected.ToString(), result.ToString());
            unitOfWorkMock.Verify(u => u.Repository<UnreadMessage>().DeleteRange(It.IsAny<List<UnreadMessage>>()), Times.Never);
        }

        [Fact]
        public async Task GetMessagesAsync_ValidParametersWithUnreadMessages_ReturnsMessageDtos()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var userId = "testUserId";
            var chatId = 1;
            var quantity = 10;
            var skip = 0;

            var messages = GetTestMessages().BuildMock();
            var unreadMessages = new List<UnreadMessage> { new UnreadMessage() }.BuildMock();


            unitOfWorkMock.Setup(u => u.Repository<Message>().GetQueryable(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IOrderedQueryable<Message>>>()))
                .Returns(messages);

            unitOfWorkMock.Setup(u => u.Repository<UnreadMessage>().GetQueryable(
                It.IsAny<Expression<Func<UnreadMessage, bool>>>(),
                It.IsAny<Func<IQueryable<UnreadMessage>, IOrderedQueryable<UnreadMessage>>>()))
                .Returns(unreadMessages);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new MessageMappingProfile()); }));

            var expected = GetTestMessageDtos();

            // Act
            var result = await service.GetMessagesAsync(userId, chatId, quantity, skip);

            // Assert
            Assert.Equal(expected.ToString(), result.ToString());
            unitOfWorkMock.Verify(u => u.Repository<UnreadMessage>().DeleteRange(It.IsAny<List<UnreadMessage>>()), Times.Once);
        }

        [Fact]
        public async Task GetMessagesAsync_InvalidQuantityOrSkip_ThrowsValidationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(
                async () => await service.GetMessagesAsync("userId", 1, quantity: -1, skip: 0)
            );

            await Assert.ThrowsAsync<ValidationException>(
                async () => await service.GetMessagesAsync("userId", 1, quantity: 0, skip: 0)
            );

            await Assert.ThrowsAsync<ValidationException>(
                async () => await service.GetMessagesAsync("userId", 1, quantity: 1, skip: -1)
            );
        }

        [Fact]
        public async Task GetMessageAsync_ExistingMessageId_ReturnsMessageDto()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var message = new List<Message> { GetTestMessages()[0] }.BuildMock();

            unitOfWorkMock.Setup(u => u.Repository<Message>().GetQueryable(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IOrderedQueryable<Message>>>()))
                .Returns(message);

            mapperMock.Setup(x => x.ConfigurationProvider)
                 .Returns(
                     () => new MapperConfiguration(
                         cfg => { cfg.AddProfile(new MessageMappingProfile()); }));

            var expected = new MessageDto
            {
                Id = 1,
                MessageText = "Test",
                SenderName = "Test",
                ChatName = "Test"
            };
            // Act
            var result = await service.GetMessageAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public async Task DeleteMessageAsync_MessageExists_DeletesMessage()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var messageId = 1;
            var existingMessage = new Message { Id = messageId };

            unitOfWorkMock.Setup(u => u.Repository<Message>().GetByIdAsync(messageId)).ReturnsAsync(existingMessage);
            unitOfWorkMock.Setup(u => u.Repository<Message>().Delete(existingMessage));
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await service.DeleteMessageAsync(messageId);

            // Assert
            unitOfWorkMock.Verify(u => u.Repository<Message>().GetByIdAsync(messageId), Times.Once);
            unitOfWorkMock.Verify(u => u.Repository<Message>().Delete(existingMessage), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMessageAsync_MessageDoesNotExist_ThrowsServiceException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var messageId = 1;

            unitOfWorkMock.Setup(u => u.Repository<Message>().GetByIdAsync(messageId)).ReturnsAsync((Message)null);

            // Act and Assert
            await Assert.ThrowsAsync<ServiceException>(() => service.DeleteMessageAsync(messageId));

            unitOfWorkMock.Verify(u => u.Repository<Message>().GetByIdAsync(messageId), Times.Once);
            unitOfWorkMock.Verify(u => u.Repository<Message>().Delete(It.IsAny<Message>()), Times.Never);
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateMessage_ExistingMessage_UpdatesMessage()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var existingMessageId = 1;
            var existingMessage = new Message { Id = existingMessageId, MessageText = "Old Message", AttachmentUrl = "https://example.com/old.jpg" };

            var updatedMessageDto = new MessageDto
            {
                Id = existingMessageId,
                MessageText = "Updated Message",
                AttachmentUrl = "https://example.com/updated.jpg"
            };

            unitOfWorkMock.Setup(u => u.Repository<Message>().GetByIdAsync(existingMessageId)).ReturnsAsync(existingMessage);
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await service.UpdateMessage(updatedMessageDto);

            // Assert
            Assert.Equal(updatedMessageDto.MessageText, existingMessage.MessageText);
            Assert.Equal(updatedMessageDto.AttachmentUrl, existingMessage.AttachmentUrl);
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateMessage_NonExistingMessage_ThrowsServiceException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var service = new MessageService(unitOfWorkMock.Object, mapperMock.Object);

            var nonExistingMessageId = 1;
            var updatedMessageDto = new MessageDto
            {
                Id = nonExistingMessageId,
                MessageText = "Updated Message",
                AttachmentUrl = "https://example.com/updated.jpg"
            };

            unitOfWorkMock.Setup(u => u.Repository<Message>().GetByIdAsync(nonExistingMessageId)).ReturnsAsync((Message)null);

            // Act and Assert
            await Assert.ThrowsAsync<ServiceException>(() => service.UpdateMessage(updatedMessageDto));
            unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Never);
        }

        private List<Message> GetTestMessages()
        {
            var message = new Message
            {
                Id = 1,
                MessageText = "Test",
                ApplicationUser = new ApplicationUser
                {
                    Id = "",
                    UserName = "Test",
                },
                Chat = new Chat
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            var testMessages = new List<Message>
            { message, message, message };
            return testMessages;
        }
        private List<MessageDto> GetTestMessageDtos()
        {
            var message = new MessageDto
            {
                Id = 1,
                MessageText = "Test",
                SenderName = "Test",
                ChatName = "Test"
            };
            var testMessages = new List<MessageDto>
            { message, message, message };
            return testMessages;
        }
    }
}
