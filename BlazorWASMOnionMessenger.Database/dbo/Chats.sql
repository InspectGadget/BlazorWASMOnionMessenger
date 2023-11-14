CREATE TABLE [dbo].[Chats]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [TypeId] INT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    [Name] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Chats_ChatTypes] FOREIGN KEY ([TypeId]) REFERENCES [ChatTypes]([Id])
)
