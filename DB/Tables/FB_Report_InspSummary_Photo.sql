CREATE TABLE [dbo].[FB_Report_InspSummary_Photo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
    [FbReportSummaryId] int NOT NULL,
	[ProductId] int NULL,
	[Photo] NVARCHAR(1000) NULL, 
	[Description] NVARCHAR(1500) NULL, 
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportSummaryId) REFERENCES [FB_Report_InspSummary](Id),
	CONSTRAINT FK_InspSummary_Photo_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
