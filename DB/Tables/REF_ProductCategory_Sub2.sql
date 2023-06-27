CREATE TABLE [dbo].[REF_ProductCategory_Sub2]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [ProductSubCategoryID] INT NOT NULL REFERENCES REF_ProductCategory_Sub, 
    [Active] BIT NOT NULL, 
    [EntityId] INT NULL,
	[Fb_Product_SubCategory2_Id] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
