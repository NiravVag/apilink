CREATE TABLE [dbo].[ES_Result_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Customer_Result_Id] int NULL,
	[EsDetailsId] int NOT NULL,
	[API_Result_Id] int null,
	CONSTRAINT ES_Result_Config_Customer_Result_Id FOREIGN KEY  ([Customer_Result_Id]) REFERENCES [dbo].[REF_INSP_CUS_Decision_Config](Id),
	CONSTRAINT ES_Result_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id),
	CONSTRAINT ES_Result_Config_API_Result_Id FOREIGN KEY ([API_Result_Id]) REFERENCES [dbo].[FB_Report_Result](Id)
)
