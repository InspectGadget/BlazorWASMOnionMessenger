CREATE TABLE [dbo].[Chats]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TypeId] INT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_Chats_ChatTypes] FOREIGN KEY ([TypeId]) REFERENCES [ChatTypes]([Id])
)
