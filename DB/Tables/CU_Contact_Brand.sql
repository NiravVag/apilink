CREATE TABLE [dbo].[CU_Contact_Brand]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] INT NOT NULL,
	[BrandId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	[EntityId] INT NULL,
	CONSTRAINT CU_Contact_Brand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([ContactId]) REFERENCES [dbo].[CU_Contact](Id),
	FOREIGN KEY([BrandId]) REFERENCES [dbo].[CU_Brand](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
