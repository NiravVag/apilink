CREATE TABLE [dbo].[QC_BL_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[Customer_Id] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
	CONSTRAINT FK_QC_BL_Customer_Customer_Id FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT FK_QC_BL_Customer_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_Customer_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)
