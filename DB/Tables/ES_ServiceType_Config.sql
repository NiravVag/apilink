CREATE TABLE [dbo].[ES_ServiceType_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [ServiceTypeId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_ServiceType_Config_ServiceTypeId FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[REF_ServiceType](Id),
	CONSTRAINT ES_ServiceType_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
