CREATE TABLE [dbo].[JOB_Schedule_Configuration]
(
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(1000),
	[Type] INT,
	[To] NVARCHAR(1500),
	[CC] NVARCHAR(1500),
	[StartDate] DATETIME NULL,
	[ScheduleInterval] INT,
	[FolderPath] NVARCHAR(1500),
	[FileName] NVARCHAR(1500),
	[EntityId] INT,
	[Active] BIT,
	[CustomerId] [nvarchar](1500) NULL,
	CONSTRAINT FK_JOB_Schedule_Configuration_Type FOREIGN KEY([Type]) REFERENCES [dbo].[JOB_Schedule_Job_Type],
	CONSTRAINT FK_JOB_Schedule_Configuration_ENTITY_ID FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_ENTITY]
)
