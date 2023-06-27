CREATE TABLE [dbo].[ES_Product_Category_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [ProductCategoryId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Product_Category_Config_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory](Id),
	CONSTRAINT ES_Product_Category_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
