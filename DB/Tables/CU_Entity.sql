CREATE TABLE [Cu_Entity]
(
	[Id] INT IDENTITY(1,1)  PRIMARY KEY,
	[CustomerId] INT NOT NULL,
	[EntityId] INT NOT NULL,
	[Active] BIT,
	[CreatedOn] DATETIME, 
	[CreatedBy] INT, 
	[DeletedOn] DATETIME,
	[DeletedBy] INT, 

	CONSTRAINT Cu_Entity_CustomerId  FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cu_Customer](Id),
	CONSTRAINT Cu_Entity_EntityId  FOREIGN KEY ([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT Cu_Entity_CreatedBy Foreign Key (CreatedBy) References IT_UserMaster(Id),
	CONSTRAINT Cu_Entity_DeletedBy Foreign Key (DeletedBy) References IT_UserMaster(Id)
)