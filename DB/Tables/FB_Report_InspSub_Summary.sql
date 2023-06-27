CREATE TABLE [dbo].[FB_Report_InspSub_Summary]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportSummaryId] int NOT NULL,	
	[Name] NVARCHAR(1000) NULL, 
	[Result] NVARCHAR(1000) NULL, 
	[ResultId] INT  NULL, 
	[Sort] INT  NULL, 
	[Remarks] NVARCHAR(MAX) NULL, 
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportSummaryId) REFERENCES [FB_Report_InspSummary](Id),
	FOREIGN KEY ([ResultId]) REFERENCES [dbo].[FB_Report_Result](Id)
)
