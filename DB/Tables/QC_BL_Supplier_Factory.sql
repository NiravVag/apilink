CREATE TABLE [dbo].[QC_BL_Supplier_Factory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[Supplier_FactoryId] INT,
	[TypeId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
	CONSTRAINT FK_QC_BL_Supplier_Factory_Supplier_Id FOREIGN KEY([Supplier_FactoryId]) REFERENCES [dbo].[SU_Supplier](Id),
	CONSTRAINT FK_QC_BL_Supplier_Factory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_Supplier_Factory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)
