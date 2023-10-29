CREATE TABLE [dbo].[UnreadMessage]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [User_id] INT NOT NULL, 
    [Message_id] INT NOT NULL, 
    CONSTRAINT [FK_UnreadMessage_Users] FOREIGN KEY ([User_id]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_UnreadMessage_Messages] FOREIGN KEY ([Message_id]) REFERENCES [Messages]([Id])
)
