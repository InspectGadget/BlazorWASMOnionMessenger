CREATE TABLE [dbo].[Chats]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Type_id] INT NOT NULL, 
    [Created_at] DATETIME NOT NULL, 
    CONSTRAINT [FK_Chats_ChatTypes] FOREIGN KEY ([Type_id]) REFERENCES [ChatTypes]([Id])
)
