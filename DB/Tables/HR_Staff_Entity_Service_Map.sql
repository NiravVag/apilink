CREATE TABLE [dbo].[HR_Staff_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StaffId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_StaffId	FOREIGN KEY([StaffId]) REFERENCES [HR_Staff](Id),
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)
