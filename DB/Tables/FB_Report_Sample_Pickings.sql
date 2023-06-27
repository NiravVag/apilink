CREATE TABLE [dbo].[FB_Report_Sample_Pickings]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SampleType] NVARCHAR(2000) NULL,
	[Destination] NVARCHAR(2000) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Sample_Pickings_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
