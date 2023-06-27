CREATE TABLE [dbo].[FB_Report_Packing_BatteryInfo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[BatteryType] NVARCHAR(2000) NULL,
	[BatteryModel] NVARCHAR(2000) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Location] NVARCHAR(2000) NULL,
	[NetWeightPerQty] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_BatteryInfo_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
