CREATE TABLE [dbo].[ES_Sup_Fact_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Supplier_OR_Factory_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Sup_Fact_Config_Supplier_OR_Factory_Id FOREIGN KEY  ([Supplier_OR_Factory_Id]) REFERENCES [dbo].[Su_Supplier](Id),
	CONSTRAINT ES_Sup_Fact_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
