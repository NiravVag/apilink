CREATE TABLE [dbo].[FB_Report_QCDetails]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportDetailId] int NOT NULL,
	[QcId] int NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportDetailId) REFERENCES [FB_Report_Details](Id)
)
