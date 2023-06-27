CREATE TABLE [dbo].[QC_BL_ProductSubCategory]
(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductSubCategoryId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_ProductSubCategoryId FOREIGN KEY ([ProductSubCategoryId]) REFERENCES [REF_ProductCategory_Sub](Id), 
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)
