CREATE TABLE [dbo].[LOG_Email_Queue]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,

	[Subject] NVARCHAR(2000)  NULL, 
	[Body] NVARCHAR(max)  NULL, 

	[SourceId] int  NULL, 
	[SourceName] NVARCHAR(200) NULL, 

	[ToList] NVARCHAR(max)  NULL, 
	[CCList] NVARCHAR(2000)  NULL, 
	[BCCList] NVARCHAR(2000)  NULL, 

    -- notstarted,success,failure from enum values - 1,2,3
	[Status] INT NULL, 
	[SendOn] DATETIME  NULL,
	[TryCount] INT NULL, 

	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
	EntityId INT null,

    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_LOG_Email_Queue_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)
