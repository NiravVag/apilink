CREATE TABLE [dbo].[ES_API_Default_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Api_Contact_Id] int NOT NULL,
	[OfficeId] int NOT NULL,
	CONSTRAINT ES_API_Default_Contacts_Api_Contact_Id FOREIGN KEY  ([Api_Contact_Id]) REFERENCES [dbo].[HR_Staff](Id)
)
