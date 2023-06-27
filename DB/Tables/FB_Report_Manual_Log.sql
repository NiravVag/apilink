CREATE TABLE [dbo].[FB_Report_Manual_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportId] [int] NULL,
	[FileUrl] [nvarchar](max) NULL,
	[UniqueId] [nvarchar](1000) NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[Active] [bit] NULL,
	[CreatedOn] [datetime] NULL DEFAULT (getdate()),
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[EntityId] INT NULL, 
    CONSTRAINT [FK_FB_Report_Manual_Log_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_FB_Report_Manual_Log_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_FB_Report_Manual_Log_Fb_Report_Id] FOREIGN KEY([FbReportId]) REFERENCES [dbo].[FB_Report_Details] ([Id]),
	Constraint FK_FB_Report_Manual_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
)