CREATE TABLE [dbo].[CU_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	[EntityId] INT NULL,
	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([CustomerId]) REFERENCES [CU_Customer](Id),
	CONSTRAINT FK_CU_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT CU_API_Services_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)