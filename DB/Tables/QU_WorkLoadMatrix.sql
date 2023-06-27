CREATE TABLE [dbo].[QU_WorkLoadMatrix]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ProductSubCategory3Id] INT NULL, 
    [PreparationTime] INT NULL, 
    [SampleSize_8h] INT NULL, 
    [Active] BIT NULL,
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [EntityId] INT NULL,
    FOREIGN KEY([ProductSubCategory3Id]) REFERENCES [REF_ProductCategory_Sub3](Id),
    FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([UpdatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
