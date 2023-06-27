CREATE TABLE [dbo].[SU_Contact_Entity_Map]
(
	ContactId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(ContactId, EntityId),
    CONSTRAINT FK_SU_Contact_Entity_Map_ContactId FOREIGN KEY(ContactId) REFERENCES Su_Contact(Id),
	CONSTRAINT FK_SU_Contact_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)
