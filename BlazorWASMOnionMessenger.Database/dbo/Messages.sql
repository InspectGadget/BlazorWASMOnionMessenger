CREATE TABLE [dbo].[Messages]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [ChatId] INT NOT NULL, 
    [SenderId] [nvarchar](450) NOT NULL, 
    [MessageText] NVARCHAR(MAX) NULL, 
    [CreatedAt] DATETIME2 DEFAULT GETDATE(), 
    [AttachmentUrl] VARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Messages_AspNetUsers] FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Messages_Chats] FOREIGN KEY ([ChatId]) REFERENCES [Chats]([Id]) ON DELETE CASCADE
)
