CREATE TABLE [dbo].[QC_BlockList]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCId] INT NOT NULL, 
	[Active] BIT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	CONSTRAINT FK_QC_BlockList_QCId FOREIGN KEY (QCId) REFERENCES [dbo].[HR_STAFF](Id),
    CONSTRAINT FK_QC_BlockList_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BlockList_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
