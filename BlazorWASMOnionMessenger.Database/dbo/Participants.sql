CREATE TABLE [dbo].[Participants]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [ChatId] INT NOT NULL, 
    [UserId] [nvarchar](450) NOT NULL, 
    [RoleId] INT NOT NULL, 
    [JoinedAt] DATETIME2 DEFAULT GETDATE(), 
    CONSTRAINT [FK_Participants_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Participants_Chats] FOREIGN KEY ([ChatId]) REFERENCES [Chats]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Participants_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]) ON DELETE CASCADE
)
