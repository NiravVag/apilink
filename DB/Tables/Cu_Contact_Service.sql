CREATE TABLE [dbo].[Cu_Contact_Service]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ContactId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	[EntityId] INT NULL,
	CONSTRAINT Cu_Contact_Service_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([ContactId]) REFERENCES [dbo].[CU_Contact](Id),
	FOREIGN KEY([ServiceId]) REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
