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
-- Init Users table
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users])
BEGIN
    -- Insert two initial users
    INSERT INTO [dbo].[Users] (Username, First_name, Last_name, Password, Phone, Created_at, Updated_at)
    VALUES
        ('user1', 'John', 'Doe', 'hashed_password_1', '123-456-7890', GETDATE(), GETDATE()),
        ('user2', 'Jane', 'Smith', 'hashed_password_2', '987-654-3210', GETDATE(), GETDATE());
END