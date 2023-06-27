CREATE TABLE [dbo].[CU_CustomerContactTypes]
(
	[contact_id] [int] NOT NULL,
	[contact_type_id] [int] NOT NULL,
	[EntityId] INT NULL,
	PRIMARY KEY([contact_id], [contact_type_id]),
	FOREIGN KEY([contact_type_id]) REFERENCES [dbo].[CU_ContactType](Id),
	FOREIGN KEY ([contact_id]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT CU_CustomerContactTypes_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
	
)
