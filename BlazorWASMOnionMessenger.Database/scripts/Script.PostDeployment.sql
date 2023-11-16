/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS (SELECT 1 FROM [dbo].[ChatTypes])
BEGIN  
    INSERT INTO [dbo].[ChatTypes] ( [Name])
    VALUES ( 'Private'),
           ( 'Group');
END  

IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles])
BEGIN  
    INSERT INTO [dbo].[Roles] ( [Name], [Description])
    VALUES ( 'Admin', 'Administrator'),
           ( 'User', 'Regular User');
END  

IF NOT EXISTS (SELECT 1 FROM [dbo].[Chats])
BEGIN
    INSERT INTO [dbo].[Chats] ( [ChatTypeId], [Name])
    VALUES
        ( 1, 'Chat 1'),
        ( 2, 'Chat 2');
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Participants])
BEGIN
    INSERT INTO [dbo].[Participants] ( [ChatId], [UserId], [RoleId])
    VALUES
        ( 1, 'fe0dc501-bcef-4e15-a664-399d8a4031e8', 1),
        ( 1, 'c214e9a5-c95f-4bc6-b69e-70381225eed0', 2),
        ( 2, '2ab8736f-d113-4f67-a3ea-b27a9484942a', 1),
        ( 2, '0e1ace14-055d-4509-b160-739c099b9a87', 2);
END

--IF NOT EXISTS (SELECT 1 FROM [dbo].[Messages])
--BEGIN
--    INSERT INTO [dbo].[Messages] ( [ChatId], [SenderId], [MessageText], [AttachmentUrl])
--    VALUES
--        ( 1, 'fe0dc501-bcef-4e15-a664-399d8a4031e8', 'Hello, this is a test message.', 'tmp'),
--        ( 1, 'c214e9a5-c95f-4bc6-b69e-70381225eed0', 'Another test message.', 'tmp'),
--        ( 2, '2ab8736f-d113-4f67-a3ea-b27a9484942a', 'Hello.', 'tmp'),
--        ( 1, '0e1ace14-055d-4509-b160-739c099b9a87', 'Another hello message.', 'tmp');
--END

--IF NOT EXISTS (SELECT 1 FROM [dbo].[UnreadMessages])
--BEGIN
--    INSERT INTO [dbo].[UnreadMessages] ( [UserId], [MessageId])
--    VALUES
--        ('c214e9a5-c95f-4bc6-b69e-70381225eed0', 1),
--        ('fe0dc501-bcef-4e15-a664-399d8a4031e8', 2),
--        ('0e1ace14-055d-4509-b160-739c099b9a87', 3),
--        ('2ab8736f-d113-4f67-a3ea-b27a9484942a', 4);
--END