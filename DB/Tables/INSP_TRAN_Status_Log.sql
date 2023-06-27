CREATE TABLE [dbo].[INSP_TRAN_Status_Log]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Booking_Id] INT NOT NULL, 
	[Status_Id] INT NOT NULL, 	
    [CreatedBy] INT NULL, 
	[CreatedOn] DATETIME NULL Default Getdate(), 	
    [ServiceDateFrom] DATETIME NULL, 
    [ServiceDateTo] DATETIME NULL, 
	[StatusChangeDate] DATETIME NULL, 	
    [EntityId] INT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([Status_Id]) REFERENCES [dbo].[INSP_Status](Id),
	FOREIGN KEY(Booking_Id) REFERENCES [INSP_Transaction](Id),
	Constraint FK_INSP_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
