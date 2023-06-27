CREATE TABLE [dbo].[FB_Report_Packing_Weight]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValues] NVARCHAR(2000) NULL,
	[WeightPackValues] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValues] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPacking] NVARCHAR(1000) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	[Unit] NVARCHAR(100) NULL,
	[PackingType] NVARCHAR(500) NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_Weight_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
