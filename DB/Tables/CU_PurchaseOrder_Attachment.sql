CREATE TABLE [dbo].[CU_PurchaseOrder_Attachment]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[GuidId] [uniqueidentifier] ROWGUIDCOL  NOT NULL Unique,
	[Po_Id] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[File] [varbinary](max) FILESTREAM  NULL,
	[UserId] [int] NOT NULL REFERENCES IT_UserMaster,
	[UploadDate] [datetime] NOT NULL,
	[Active] [bit] NOT NULL, 
	FOREIGN KEY (Po_Id) REFERENCES [dbo].[CU_PurchaseOrder] (Id)
)



