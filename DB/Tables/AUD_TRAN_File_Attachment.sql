CREATE TABLE [dbo].[AUD_TRAN_File_Attachment]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Audit_Id] INT NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Active] BIT NOT NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [UniqueId] NVARCHAR(1000) NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	FbMissionUrlId int,
    FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	
)
