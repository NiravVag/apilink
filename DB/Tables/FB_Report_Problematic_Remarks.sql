CREATE TABLE [dbo].[FB_Report_Problematic_Remarks]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportSummaryId] int NOT NULL,
	[ProductId] int NULL,
	[Remarks] NVARCHAR(max) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[Sub_Category] nvarchar(1000) NULL,
	[Sub_Category2] nvarchar(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[CustomerRemarkCode] NVARCHAR(1000) NULL, 
    FOREIGN KEY(FbReportSummaryId) REFERENCES [FB_Report_InspSummary](Id),
	CONSTRAINT FK_Problematic_Remarks_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
