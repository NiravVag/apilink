﻿CREATE TABLE [dbo].[DF_ControlTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL
)
