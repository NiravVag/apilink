﻿CREATE TABLE [dbo].[REF_Billing_Entity]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL
)
