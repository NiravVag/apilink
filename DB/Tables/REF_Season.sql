CREATE TABLE [dbo].[REF_Season]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL,
	[Code] nvarchar(200) NOT NULL,
	[Default] BIT NULL,
	[Active] BIT NOT NULL,
	[EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)
)
