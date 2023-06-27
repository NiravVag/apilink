CREATE TABLE [dbo].[CU_CheckPoints_Brand]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CheckpointId] INT NOT NULL, 
    [BrandId] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	[EntityId] INT NULL,
	CONSTRAINT CU_CheckPoints_Brand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([CheckpointId]) REFERENCES CU_CheckPoints(Id),
	FOREIGN KEY([CreatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([UpdatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([BrandId]) REFERENCES CU_Brand(Id)
)
