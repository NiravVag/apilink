CREATE TABLE [dbo].[AUD_TRAN_SU_Contact]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Contact_Id] INT NOT NULL, 
    [Audit_Id] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	,
    FOREIGN KEY(Contact_Id) REFERENCES [SU_Contact](Id),
	FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id)
	
)
