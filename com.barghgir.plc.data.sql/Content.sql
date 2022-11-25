CREATE TABLE [dbo].[Content]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Title] NVARCHAR(50) NULL, 
    [Source] VARCHAR(100) NULL, 
    [DurationSeconds] INT NULL
)
