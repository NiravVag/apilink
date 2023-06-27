CREATE TABLE [dbo].[AUD_TRAN_ServiceType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ServiceType_Id] INT NOT NULL, 
	[Audit_Id] INT NOT NULL,
	[Active] BIT NOT NULL, 
	[CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	,
    FOREIGN KEY(ServiceType_Id) REFERENCES [REF_ServiceType](Id),
	FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id)
)
