CREATE TABLE [HR_OfficeControl](
	StaffId INT NOT NULL,
	LocationId INT NOT NULL,
	EntityId INT null,
	PRIMARY KEY(StaffId, LocationId),
	FOREIGN KEY(LocationId) REFERENCES [dbo].[REF_Location](Id),
	FOREIGN KEY(StaffId) REFERENCES [dbo].[HR_STaff](Id),
	CONSTRAINT FK_HR_OfficeControl_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)
