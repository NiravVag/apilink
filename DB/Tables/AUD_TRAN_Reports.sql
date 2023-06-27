CREATE TABLE [dbo].[AUD_TRAN_Reports]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE, 
	[Audit_Id] INT NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[File] VARBINARY(MAX) FILESTREAM NULL,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Active] BIT NOT NULL, 
    FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id)	
)
