CREATE TABLE [dbo].[FB_Report_QualityPlan_MeasurementDefectsSize](
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[QualityPlanId] [int] NULL,
		[Size] [nvarchar](500) NULL,
		[Quantity] [int] NULL,
		CONSTRAINT [FB_Report_QualityPlan_MeasurementDefectsSize_QualityPlanId] FOREIGN KEY([QualityPlanId]) REFERENCES [dbo].[FB_Report_QualityPlan] ([Id])
)