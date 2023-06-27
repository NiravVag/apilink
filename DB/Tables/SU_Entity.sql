CREATE TABLE [SU_Entity]
(
	[Id] INT IDENTITY(1,1)  PRIMARY KEY,
	[SupplierId] INT NOT NULL,
	[EntityId] INT NOT NULL,
	[Active] BIT,
	[CreatedOn] DATETIME, 
	[CreatedBy] INT, 
	[DeletedOn] DATETIME,
	[DeletedBy] INT, 

	CONSTRAINT Su_Entity_Supplierid  FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Su_Supplier](Id),
    CONSTRAINT Su_Entity_EntityId  FOREIGN KEY ([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT Su_Entity_CreatedBy Foreign Key (CreatedBy) References IT_UserMaster(Id),
	CONSTRAINT Su_Entity_DeletedBy Foreign Key (DeletedBy) References IT_UserMaster(Id)
)