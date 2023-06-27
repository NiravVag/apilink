CREATE TABLE [dbo].[ES_FA_Country_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Factory_CountryId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_FA_Country_Config_Factory_CountryId FOREIGN KEY ([Factory_CountryId]) REFERENCES [dbo].[REF_Country](Id),
	CONSTRAINT ES_FA_Country_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
