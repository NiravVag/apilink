CREATE TABLE [dbo].[LOG_Email_Queue_Attachments]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[GuidId] [uniqueidentifier] ROWGUIDCOL  NOT NULL Unique,

	[Email_Queue_Id] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[File] [varbinary](max) FILESTREAM  NULL,	

	[Active] [bit] NOT NULL,  
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] DATETIME NULL,   

	[EntityId] INT NULL, 
    FOREIGN KEY ([Email_Queue_Id]) REFERENCES [dbo].[LOG_Email_Queue](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	Constraint FK_LOG_Email_Queue_Attachments_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)
