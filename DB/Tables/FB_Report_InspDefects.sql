CREATE TABLE [dbo].[FB_Report_InspDefects]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportDetailId] int NOT NULL,
	[InspPoTransactionId] int NOT NULL,
	[Description] NVARCHAR(1500) NULL, 
	[Position] NVARCHAR(100) NULL, 
    [Critical] int NULL,
	[Major] int NULL,
	[Minor] int NULL,
	[DefectId] INT NULL,
	[CategoryId] INT NULL,
	[Qty_Reworked] int NULL,
	[Qty_Replaced] int NULL,
	[Qty_Rejected] int NULL,
	[CategoryName] NVARCHAR(3000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	DefectCheckPoint int,
	FOREIGN KEY(FbReportDetailId) REFERENCES [FB_Report_Details](Id),
	FOREIGN KEY(InspPoTransactionId) REFERENCES [INSP_PurchaseOrder_Transaction](Id)
)
