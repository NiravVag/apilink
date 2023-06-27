CREATE TABLE [dbo].[INV_REF_FileType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] [nvarchar](200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [IsUpload] BIT NULL, 
    [Sort] INT NULL
)
