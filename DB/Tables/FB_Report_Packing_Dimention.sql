CREATE TABLE [dbo].[FB_Report_Packing_Dimention]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,	
	[PackingType] NVARCHAR(2000) NULL,
	[SpecClientValuesL] NVARCHAR(2000) NULL,
	[SpecClientValuesW] NVARCHAR(2000) NULL,
	[SpecClientValuesH] NVARCHAR(2000) NULL,
	[PrintedPackValuesL] NVARCHAR(2000) NULL,
	[PrintedPackValuesW] NVARCHAR(2000) NULL,
	[PrintedPackValuesH] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValuesL] NVARCHAR(2000) NULL,
	[MeasuredValuesW] NVARCHAR(2000) NULL,
	[MeasuredValuesH] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPacking] NVARCHAR(1000) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	[Unit] NVARCHAR(100) NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_Dimention_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
	
)
