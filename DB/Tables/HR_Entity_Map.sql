CREATE TABLE [dbo].[HR_Entity_Map]
(
	StaffId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(StaffId, EntityId),
    CONSTRAINT FK_HR_Entity_Map_ContactId FOREIGN KEY(StaffId) REFERENCES HR_staff(Id),
	CONSTRAINT FK_HR_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)
