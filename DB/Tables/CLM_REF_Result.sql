﻿CREATE TABLE [dbo].[CLM_REF_Result]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL, 
    [IsValidate] BIT NULL, 
    [IsFinal] BIT NULL
)