CREATE TABLE [dbo].[ES_API_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Api_Contact_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_API_Contacts_Customer_Api_Contact_Id FOREIGN KEY  ([Api_Contact_Id]) REFERENCES [dbo].[HR_Staff](Id),
	CONSTRAINT ES_API_Contacts_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)
