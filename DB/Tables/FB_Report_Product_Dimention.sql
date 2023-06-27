CREATE TABLE [dbo].[FB_Report_Product_Dimention]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValuesL] NVARCHAR(2000) NULL,
	[SpecClientValuesW] NVARCHAR(2000) NULL,
	[SpecClientValuesH] NVARCHAR(2000) NULL,
	[DimensionPackValuesL] NVARCHAR(2000) NULL,
	[DimensionPackValuesW] NVARCHAR(2000) NULL,
	[DimensionPackValuesH] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValuesL] NVARCHAR(2000) NULL,
	[MeasuredValuesW] NVARCHAR(2000) NULL,
	[MeasuredValuesH] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPack]  NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	[Description] nvarchar(1000) , 
	[Unit] nvarchar(100),
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Product_Dimention_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
	
)
