﻿CREATE TABLE [dbo].[ES_Type]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL, 
    [Active] BIT NOT NULL	
)