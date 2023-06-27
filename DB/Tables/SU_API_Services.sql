CREATE TABLE [dbo].[SU_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SupplierId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([SupplierId]) REFERENCES [SU_Supplier](Id),
	CONSTRAINT FK_SU_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id)
)
