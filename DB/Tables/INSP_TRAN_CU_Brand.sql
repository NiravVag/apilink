CREATE TABLE [dbo].[INSP_TRAN_CU_Brand]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Inspection_Id] INT NOT NULL, 
	[Brand_Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([Brand_Id]) REFERENCES [dbo].[CU_Brand](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
