CREATE TABLE [dbo].[ES_CU_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [DepartmentId] int  NULL,
	[BrandId] int  NULL,
	[CollectionId] int  NULL,
	[BuyerId] int  NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_CU_Config_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),
	CONSTRAINT ES_CU_Config_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),
	CONSTRAINT ES_CU_Config_CollectionId FOREIGN KEY ([CollectionId]) REFERENCES [dbo].[CU_Collection](Id),
	CONSTRAINT ES_CU_Config_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),
	CONSTRAINT ES_CU_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
