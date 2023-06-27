CREATE TABLE [dbo].[FB_Report_Product_Weight]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValues] NVARCHAR(2000) NULL,
	[WeightPackValues] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValues] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPack] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	[Description] nvarchar(1000) , 
	[Unit] nvarchar(100),
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Product_Weight_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
