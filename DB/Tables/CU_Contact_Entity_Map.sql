CREATE TABLE [dbo].[CU_Contact_Entity_Map]
(
	ContactId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(ContactId, EntityId),
    CONSTRAINT FK_CU_Contact_Entity_Map_ContactId FOREIGN KEY(ContactId) REFERENCES Cu_Contact(Id),
	CONSTRAINT FK_CU_Contact_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)
