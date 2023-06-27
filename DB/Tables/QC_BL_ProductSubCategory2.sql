CREATE TABLE [dbo].[QC_BL_ProductSubCategory2]
(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductSubCategory2Id] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductSubCategory2_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductSubCategory2_ProductCategoryId FOREIGN KEY ([ProductSubCategory2Id]) REFERENCES [REF_ProductCategory_Sub2](Id),
	CONSTRAINT FK_QC_BL_ProductSubCategory2_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)
