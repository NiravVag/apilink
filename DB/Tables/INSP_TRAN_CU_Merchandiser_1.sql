CREATE TABLE [dbo].[INSP_TRAN_CU_Merchandiser]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Inspection_Id] INT NOT NULL, 
    [Merchandiser_Id] INT NOT NULL, 
    [Active] BIT NOT NULL,
	[CreatedOn] DATETIME NULL, 
    [CreatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Merchandiser_Id]) REFERENCES [dbo].[CU_Contact](Id)
)
