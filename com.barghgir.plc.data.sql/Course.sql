CREATE TABLE [dbo].[Course]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Title] NVARCHAR(50) NULL, 
    [Subtitle] NVARCHAR(50) NULL, 
    [Category] NVARCHAR(50) NULL, 
    [ImageId] INT NULL, 
    [ContentTypeId] INT NULL
)
