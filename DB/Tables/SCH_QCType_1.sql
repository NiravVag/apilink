
CREATE TABLE [dbo].[SCH_QCType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Type] NVARCHAR(100) NOT NULL, 
	[Active] bit,
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL
	FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (ModifiedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
