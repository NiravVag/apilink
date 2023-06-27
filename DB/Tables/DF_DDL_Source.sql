CREATE TABLE [dbo].[DF_DDL_Source]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] nvarchar(50) NOT NULL,
	[Type] INT NOT NULL,
	[Active] bit NOT NULL,
	[ParentId] INT NULL,
	FOREIGN KEY(Type) REFERENCES DF_DDL_SourceType(Id)
)
