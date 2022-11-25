CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL, 
    [Email] VARCHAR(150) NOT NULL , 
    [Password] VARCHAR(50) NULL DEFAULT null, 
    [VerifyDate] DATETIME NULL DEFAULT null, 
    [LockDate] DATETIME NULL DEFAULT null, 
    [CreateDate] DATETIME NULL DEFAULT getdate(), 
    [LastSignInDate] DATETIME NULL DEFAULT null, 
    [LastPasswordUpdate] DATETIME NULL DEFAULT null, 
    [VerifyCode] NCHAR(6) NULL DEFAULT null, 
    [IsAdmin] BIT NULL DEFAULT 1, 
    [FailedSignInCount] INT NOT NULL DEFAULT 0, 
    [LastFailedSignInDate] DATETIME NULL DEFAULT null
)
