CREATE TABLE [dbo].[CU_Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductID] NVARCHAR(200) NOT NULL, 
    [Product Description] NVARCHAR(3500) NOT NULL, 
    [CustomerID] INT NOT NULL, 
    [Barcode] NVARCHAR(15) NULL, 
    [ProductCategory] INT NULL REFERENCES REF_ProductCategory, 
    [ProductSubCategory] INT NULL REFERENCES REF_ProductCategory_Sub, 
    [ProductCategorySub2] INT NULL REFERENCES REF_ProductCategory_Sub2, 
    [Remarks] NVARCHAR(1000) NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedTime] DATETIME NOT NULL, 
	[DeletedBy] INT NULL, 
    [DeletedTime] DATETIME NULL, 
    [Fb_Cus_Prod_Id] INT NULL, 
	[FactoryReference] NVARCHAR(1000) NULL, 
	IsNewProduct BIT NULL,
	IsMS_Chart BIT NULL,
	IsStyle BIT NULL,
    FOREIGN KEY([CustomerID]) REFERENCES CU_Customer(Id),

)
