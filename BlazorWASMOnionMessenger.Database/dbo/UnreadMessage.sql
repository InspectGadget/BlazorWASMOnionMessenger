CREATE TABLE [dbo].[UnreadMessage]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] [nvarchar](450) NOT NULL, 
    [MessageId] INT NOT NULL, 
    CONSTRAINT [FK_UnreadMessage_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_UnreadMessage_Messages] FOREIGN KEY ([MessageId]) REFERENCES [Messages]([Id])
)
