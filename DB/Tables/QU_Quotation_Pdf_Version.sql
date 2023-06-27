CREATE TABLE [dbo].[QU_Quotation_Pdf_Version]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UniqueId] [nvarchar](1000) NULL,	
	[FileName] [nvarchar](500) NULL,	
	[FileUrl] [nvarchar](max) NULL,
	[UserId] [int]  NULL,
	[Quotation_Id] [int]  NULL,
	[UploadDate] [datetime]  NULL,
	[SendToClient] [bit]  NULL,   
    CONSTRAINT FK_QU_Quotation_Pdf_Version_Quotation_Id FOREIGN KEY ([Quotation_Id]) REFERENCES [dbo].[QU_Quotation](Id),
    CONSTRAINT FK_QU_Quotation_Pdf_Version_UserId FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id)
)