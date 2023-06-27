CREATE TABLE [dbo].[REF_Town]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CountyId] INT NOT NULL, 
    [TownName] NVARCHAR(500) NOT NULL, 
    [TownCode] NVARCHAR(500) NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL
	FOREIGN KEY (CountyId) REFERENCES[dbo].[REF_COUNTY](Id),
	FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (ModifiedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
