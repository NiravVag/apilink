﻿CREATE TABLE [dbo].[EC_ExpClaimStatus]
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
	[Description] NVARCHAR(100) NOT NULL,
	[TranId] INT NULL,
	Active BIT NOT NULL DEFAULT(1), 
    [EntityId] INT NULL,
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)