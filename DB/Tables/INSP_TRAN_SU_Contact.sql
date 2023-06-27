CREATE TABLE [dbo].[INSP_TRAN_SU_Contact]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
    [Inspection_Id] INT NOT NULL, 
    [Contact_Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedOn] DATETIME NULL, 
    [CreatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	FOREIGN KEY ([Contact_Id]) REFERENCES [dbo].[SU_Contact](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id)
)
