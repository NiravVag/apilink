CREATE TABLE [dbo].[CU_Product_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ProductId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	[EntityId] INT NULL,
	CONSTRAINT CU_Product_API_Services_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([ProductId]) REFERENCES [dbo].[CU_Products](Id),
	CONSTRAINT FK_CU_Product_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
