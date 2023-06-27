CREATE TABLE [dbo].[FB_Report_QualityPlan_MeasurementDefectsPOM](
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[QualityPlanId] [int] NULL,
		[CodePOM] [nvarchar](500) NULL,
		[POM] [nvarchar](500) NULL,
		[CriticalPOM] [nvarchar](500) NULL,
		[Quantity] [int] NULL,
		[SpecZone] [nvarchar](500) NULL,
		CONSTRAINT [FB_Report_QualityPlan_MeasurementDefectsPOM_QualityPlanId] FOREIGN KEY([QualityPlanId]) REFERENCES [dbo].[FB_Report_QualityPlan] ([Id])
)
