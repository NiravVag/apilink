CREATE TABLE [dbo].[CU_Product_File_Attachment]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UniqueId] nvarchar(1000) null,
	[Product_Id] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,	
	[FileUrl] nvarchar(max) null,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Active] [bit] NOT NULL, 
    [Booking_Id] INT NULL, 
    [DeletedBy] INT NULL, 	
    [DeletedOn] DATETIME NULL, 
	FileType_Id int null ,
	FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[CU_Products](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Booking_Id]) REFERENCES [dbo].[INSP_TRANSACTION](Id),
	CONSTRAINT FK_CU_Product_File_Attachment_FileType_Id FOREIGN KEY ([FileType_Id]) REFERENCES [dbo].[CU_Product_FileType](Id)
)



