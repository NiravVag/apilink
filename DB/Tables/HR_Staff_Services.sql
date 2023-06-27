CREATE TABLE [dbo].[HR_Staff_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StaffId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	CONSTRAINT FK_HR_Staff_Services_CreatedBy	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_HR_Staff_Services_DeletedBy	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_HR_Staff_Services_StaffId	FOREIGN KEY([StaffId]) REFERENCES [HR_Staff](Id),
	CONSTRAINT FK_Staff_API_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id)
)
