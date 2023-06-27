﻿CREATE TABLE [dbo].[EC_ExpensesTypes](
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Description] NVARCHAR(200) NOT NULL,
	[Active] BIT NOT NULL DEFAULT(1),
	[TypeTransId] INT NULL,
	 [EntityId] INT NULL,
	 IsTravel BIT NOT NULL DEFAULT(1) 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)