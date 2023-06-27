CREATE TABLE [dbo].[FB_Booking_RequestLog]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[BookingId] INT NULL,
	[MissionId] INT NULL,
	[ReportId] INT NULL,
	[RequestUrl] NVARCHAR(1000) NULL,
	[MissionProductId] INT NULL,
	[LogInformation] NVARCHAR(MAX) NULL,      
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL, 
    [AccountId] INT NULL, 
    [EntityId] INT NULL,
	ServiceId int,
	Constraint FK_FB_Booking_RequestLog_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
