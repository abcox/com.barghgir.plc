CREATE TABLE [dbo].[CourseContent]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [CourseId] INT NULL, 
    [ContentId] INT NULL, 
    [SortOrder] INT NULL
)
