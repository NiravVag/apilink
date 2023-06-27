CREATE TABLE [dbo].[SU_Contact_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ContactId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([ContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT FK_SU_Contact_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
