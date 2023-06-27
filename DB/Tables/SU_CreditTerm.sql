CREATE TABLE [dbo].[SU_CreditTerm]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(1000) NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL
	FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
