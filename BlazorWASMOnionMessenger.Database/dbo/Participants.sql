CREATE TABLE [dbo].[Participants]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Chat_id] INT NOT NULL, 
    [User_id] INT NOT NULL, 
    [Role_id] INT NOT NULL, 
    [Joined_at] DATETIME NOT NULL, 
    CONSTRAINT [FK_Participants_Users] FOREIGN KEY ([User_id]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_Participants_Chats] FOREIGN KEY ([Chat_id]) REFERENCES [Chats]([Id]), 
    CONSTRAINT [FK_Participants_Roles] FOREIGN KEY ([Role_id]) REFERENCES [Roles]([Id])
)
