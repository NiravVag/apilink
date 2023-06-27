﻿CREATE TABLE [dbo].[CU_PR_PriceCategory]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[PriceCategory_Id] INT,
	[Active] BIT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	[EntityId] INT NULL,
	CONSTRAINT CU_PR_PriceCategory_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([PriceCategory_Id]) REFERENCES [dbo].[CU_PriceCategory](Id),
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
