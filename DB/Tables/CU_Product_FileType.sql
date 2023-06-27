CREATE TABLE [dbo].[CU_Product_FileType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(200) NOT NULL, 
	[Sort] INT NULL,
	[Active] BIT
)
