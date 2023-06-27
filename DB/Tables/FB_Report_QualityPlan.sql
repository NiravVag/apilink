CREATE TABLE [dbo].[FB_Report_QualityPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportDetailsId] [int] NULL,
	[Title] [nvarchar](500) NULL,
	[TotalDefectiveUnits] [int] NULL,
	[Result] [nvarchar](500) NULL,
	[TotalQtyMeasurmentDefects] [int] NULL,
	[CreatedOn] [datetime] NULL,
	TotalPiecesMeasurmentDefects nvarchar(100),
	SampleInspected nvarchar(100),
	ActualMeasuredSampleSize nvarchar(100),
	CONSTRAINT [FK_FB_Report_QualityPlan_FbReportDetailsId] FOREIGN KEY([FbReportDetailsId]) REFERENCES [dbo].[FB_Report_Details] ([Id])
)