CREATE TABLE [dbo].[CU_Collection]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME not NULL default getdate(), 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	[EntityId] INT NULL,
	CONSTRAINT CU_Collection_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY(CustomerId) REFERENCES [Cu_Customer](Id), 	
	CONSTRAINT FK_CU_Collection_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_CU_Collection_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_CU_Collection_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	
)
