﻿CREATE TABLE [dbo].[DF_Attributes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	[DataType] NVARCHAR(10) NOT NULL,
	[Active] BIT NOT NULL
)