CREATE TABLE [dbo].[CU_Contact_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_ContactId	FOREIGN KEY([ContactId]) REFERENCES [dbo].[Cu_Contact](Id),
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)

