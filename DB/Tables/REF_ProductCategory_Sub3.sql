CREATE TABLE [dbo].[REF_ProductCategory_Sub3]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(1000) NOT NULL, 
    [ProductSubCategory2Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [EntityId] INT NULL,
    FOREIGN KEY([ProductSubCategory2Id]) REFERENCES [REF_ProductCategory_Sub2](Id),
    FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([UpdatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
