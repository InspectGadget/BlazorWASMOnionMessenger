CREATE TABLE [dbo].[UnreadMessages]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [UserId] [nvarchar](450) NOT NULL, 
    [MessageId] INT NOT NULL, 
    CONSTRAINT [FK_UnreadMessages_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_UnreadMessages_Messages] FOREIGN KEY ([MessageId]) REFERENCES [Messages]([Id])
)
