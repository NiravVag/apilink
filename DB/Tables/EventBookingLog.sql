CREATE TABLE [dbo].[EventBookingLog]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Booking_Id] INT NULL, 
	[Audit_Id] INT  NULL, 
	[Quotation_Id] INT NULL,
	[Status_Id] INT NULL, 	
	[LogInformation] NVARCHAR(MAX) NULL, 
    [CreatedBy] INT NOT NULL, 
	[CreatedOn] DATETIME NOT NULL, 	
    [EntityId] INT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	Constraint FK_EventBookingLog_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
