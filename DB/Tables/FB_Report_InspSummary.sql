CREATE TABLE [dbo].[FB_Report_InspSummary]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportDetailId] int not null,
	[FbReportInspsumTypeId] int not null,
	[Name] NVARCHAR(1000) NULL, 
	[Result] NVARCHAR(1000) NULL, 
	[Sort] INT  NULL, 
	[ResultId] INT  NULL, 
	[Remarks] NVARCHAR(max) NULL, 
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	ScoreValue nvarchar(100),
	ScorePercentage nvarchar(100),
	FOREIGN KEY(FbReportDetailId) REFERENCES [FB_Report_Details](Id),	
	FOREIGN KEY(FbReportInspsumTypeId) REFERENCES [FB_Report_InspSummary_Type](Id),
	FOREIGN KEY ([ResultId]) REFERENCES [dbo].[FB_Report_Result](Id)
)
