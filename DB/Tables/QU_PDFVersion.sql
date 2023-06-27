CREATE TABLE [dbo].[QU_PDFVersion](
	[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL PRIMARY KEY, 
	[FileName] [nvarchar](200) NOT NULL,
	[File] VARBINARY(MAX) FILESTREAM NULL,
	[UserId] [int] NOT NULL,
	[GenerateDate] [datetime] NOT NULL,
	[QuotationId] INT NOT NULL,
	FOREIGN KEY ([QuotationId]) REFERENCES [dbo].[QU_Quotation](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id)	
)
