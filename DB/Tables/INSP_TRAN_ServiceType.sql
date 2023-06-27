CREATE TABLE [dbo].[INSP_TRAN_ServiceType]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
    [Inspection_Id] INT NOT NULL, 
    [ServiceType_Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	FOREIGN KEY ([ServiceType_Id]) REFERENCES [dbo].[REF_ServiceType](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id)
)
