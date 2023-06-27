CREATE TABLE [dbo].[INV_TRAN_Files]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[InvoiceId] INT,
	[InvoiceNo] NVARCHAR(1000),
	[FileName] NVARCHAR(1000),
	[FileType] INT,
	[UniqueId] NVARCHAR(MAX),
	[FilePath] NVARCHAR(1000),
	[CreatedBy] INT,	
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT,
	[DeletedOn] DATETIME NULL,
	[Active] BIT,	
	CONSTRAINT FK_INV_TRAN_Files_InvoiceId FOREIGN KEY([InvoiceId]) REFERENCES [dbo].[INV_AUT_TRAN_Details],
	CONSTRAINT FK_INV_TRAN_Files_FileType FOREIGN KEY([FileType]) REFERENCES [dbo].[INV_REF_FileType],
	CONSTRAINT FK_INV_TRAN_Files_CreatedBy FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster],
	CONSTRAINT FK_INV_TRAN_Files_DeletedBy FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster]
)
