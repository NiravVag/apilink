CREATE TABLE [dbo].[FB_Report_PackingInfo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[MaterialType] NVARCHAR(2000) NULL,
	[PackagingDesc] NVARCHAR(max) NULL,
	[PieceNo] FLOAT NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Location] NVARCHAR(2000) NULL,
	[NetWeightPerQty] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_PackingInfo_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
