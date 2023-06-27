
CREATE TABLE [dbo].[HR_FileAttachment](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE, 
	[FullFileName] [nvarchar](200) NOT NULL,
	[File] VARBINARY(MAX) FILESTREAM NULL,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[staff_id] [int] NOT NULL,
	[FileTypeId] [int] NOT NULL,
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([FileTypeId]) REFERENCES [dbo].[HR_FileType](Id) )


