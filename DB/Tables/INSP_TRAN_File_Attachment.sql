CREATE TABLE [dbo].[INSP_TRAN_File_Attachment]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UniqueId] nvarchar(1000) null,
    [Inspection_Id] INT NOT NULL, 
    [FileName] NVARCHAR(500) NOT NULL, 
    [FileUrl] nvarchar(max) null,
    [UserId] INT NOT NULL, 
    [UploadDate] DATETIME NOT NULL,
	[Active] BIT NOT NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [Fb_Id] INT NULL, 
    FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)