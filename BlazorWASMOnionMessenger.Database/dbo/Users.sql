CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Username] VARCHAR(30) NOT NULL, 
    [First_name] NVARCHAR(30) NOT NULL, 
    [Last_name] NVARCHAR(30) NOT NULL, 
    [Password] VARCHAR(40) NOT NULL, 
    [Phone] VARCHAR(16) NOT NULL, 
    [Created_at] DATETIME NOT NULL, 
    [Updated_at] DATETIME NOT NULL
)
