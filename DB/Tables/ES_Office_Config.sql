CREATE TABLE [dbo].[ES_Office_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [OfficeId] int  NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Office_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id),
	CONSTRAINT ES_Office_Config_Office_Id FOREIGN KEY (OfficeId) REFERENCES [dbo].[Ref_Location](Id)
)
