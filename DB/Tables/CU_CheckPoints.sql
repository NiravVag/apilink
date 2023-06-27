CREATE TABLE [dbo].[CU_CheckPoints]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [CheckpointTypeId] INT NOT NULL, 
    [ServiceId] INT NOT NULL,
	[Remarks] NVARCHAR(MAX) NULL, 
    [Active] BIT NOT NULL, 
	[CustomerId] INT NOT NULL,
	[CreatedOn] DATETIME NULL default GETDATE(), 	
    [CreatedBy] INT NULL, 
	    [ModifiedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
	[DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 	
	[EntityId] INT NULL,
    CONSTRAINT FK_CPCheckpointTypeId FOREIGN KEY([CheckpointTypeId]) REFERENCES [dbo].[CU_CheckPointType](Id),
	CONSTRAINT FK_CPServiceId FOREIGN KEY([ServiceId]) REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_CPCustomerId FOREIGN KEY(CustomerId) REFERENCES [CU_Customer](Id),
	CONSTRAINT FK_CPCreatedBy FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
CONSTRAINT FK_CPModifiedBy FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id),
CONSTRAINT FK_CPDeletedBy FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id),
CONSTRAINT CU_CheckPoints_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
