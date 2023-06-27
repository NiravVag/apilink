CREATE TABLE [dbo].[REF_ProductCategory_Sub]
(
	[Id] INT NOT NULL IDENTITY , 
    [Name] NVARCHAR(50) NOT NULL, 
    [ProductCategoryID] INT NOT NULL REFERENCES REF_ProductCategory, 
    [Active] BIT NOT NULL, 
    [EntityId] INT NULL, 
	[Fb_Product_SubCategory_Id] INT NULL, 
    CONSTRAINT [PK_REF_ProductCategory_Sub] PRIMARY KEY ([Id]),
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)
