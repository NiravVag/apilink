CREATE TABLE [dbo].[QC_BL_ProductCatgeory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductCategoryId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductCatgeory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductCatgeory_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [REF_ProductCategory](Id), 
	CONSTRAINT FK_QC_BL_ProductCatgeory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)
