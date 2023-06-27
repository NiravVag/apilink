CREATE TABLE LOG_Booking_FB_Queue(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BookingId] [int] NOT NULL,
	[FbBookingSyncType] [int] NOT NULL,
	[TryCount] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsMissionUpdated] [bit] NULL,
	[Status] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	CONSTRAINT FK_LOG_Booking_FB_Queue_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_LOG_Booking_FB_Queue_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id),
)