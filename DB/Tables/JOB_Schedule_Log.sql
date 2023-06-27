CREATE TABLE [dbo].[JOB_Schedule_Log]
(
	[Id] int not null primary key identity(1,1),
	[Booking_Id] INT, 
	[Report_Id] INT ,	
	[Schedule_Type] INT ,
	[FileName] nvarchar(500), 
	[CreatedOn] datetime
    CONSTRAINT FK_JOB_Schedule_Log_Booking_Id FOREIGN KEY([Booking_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	[EntityId] INT NULL, 
    CONSTRAINT FK_JOB_Schedule_Log_Report_Id FOREIGN KEY([Report_Id]) REFERENCES [dbo].[FB_Report_Details](Id),
	Constraint FK_JOB_Schedule_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
