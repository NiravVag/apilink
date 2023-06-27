CREATE TABLE [dbo].[CU_PriceCategory_PCSub2]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	 CustomerId int not null,
	ProductSubCategoryId2 int not null,
	PriceCategoryId int not null,
	Active bit, 
	CreatedOn datetime default getdate(),
	[EntityId] INT NULL,
	CONSTRAINT CU_PriceCategory_PCSub2_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT CU_PriceCategory_PCSub2_CustomerId  FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cu_Customer](Id),
	CONSTRAINT CU_PriceCategory_PCSub2_ProductSubCategoryId2 FOREIGN KEY ([ProductSubCategoryId2]) REFERENCES [dbo].[REF_ProductCategory_Sub2](Id),
	CONSTRAINT CU_PriceCategory_PCSub2_PriceCategoryId FOREIGN KEY ([PriceCategoryId]) REFERENCES [dbo].[CU_PriceCategory](Id),
)