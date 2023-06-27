CREATE TABLE [dbo].[INSP_DF_DDL_Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BookingId] INT NOT NULL,
	[ControlConfigurationId] INT NOT NULL,
	[Value] NVARCHAR(100) NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DateTime NOT NULL,
	[UpdatedBy] INT NULL,
	[UpdatedOn] DateTime NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY(BookingId) REFERENCES INSP_Transaction(Id),
	FOREIGN KEY(ControlConfigurationId) REFERENCES DF_CU_Configuration(Id),
	FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(UpdatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id),
)
