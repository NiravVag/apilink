CREATE TABLE [dbo].[ES_CU_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Customer_Contact_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_CU_Contacts_Customer_Contact_Id FOREIGN KEY  ([Customer_Contact_Id]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT ES_CU_Contacts_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
