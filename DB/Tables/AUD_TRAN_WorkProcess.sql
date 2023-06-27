CREATE TABLE [dbo].[AUD_TRAN_WorkProcess]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Audit_Id] INT NOT NULL, 
    [WorkProcess_Id] INT NOT NULL, 
	[CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
	[Active] BIT NOT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	,
	FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY(WorkProcess_Id) REFERENCES [AUD_WorkProcess](Id)
)
