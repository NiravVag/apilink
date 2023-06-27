CREATE TABLE [dbo].[HR_REF_Band]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL,
	[Active] BIT NOT NULL,
	[Sort] INT  NULL, 	
)
