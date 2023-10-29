CREATE TABLE [dbo].[Messages]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Chat_id] INT NOT NULL, 
    [Sender_id] INT NOT NULL, 
    [Message_text] NVARCHAR(MAX) NULL, 
    [Created_at] DATETIME NOT NULL, 
    [Attachment_url] VARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Messages_Users] FOREIGN KEY ([Sender_id]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_Messages_Chats] FOREIGN KEY ([Chat_id]) REFERENCES [Chats]([Id])
)
