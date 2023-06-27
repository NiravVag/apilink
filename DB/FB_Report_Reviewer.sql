CREATE TABLE [dbo].[FB_Report_Reviewer]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ReviewerId] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id)
)
