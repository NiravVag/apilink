CREATE TABLE [dbo].[SU_Contact_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_ContactId	FOREIGN KEY([ContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)
