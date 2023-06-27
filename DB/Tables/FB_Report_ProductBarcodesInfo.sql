CREATE TABLE [dbo].[FB_Report_ProductBarcodesInfo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductId] INT NULL,
	[FbReportId] INT NOT NULL,
	[BarCode] NVARCHAR(500) NULL,	
	[Description] NVARCHAR(2000) NULL,
    CONSTRAINT FK_FB_Report_ProductBarcodesInfo_FbReportId	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FB_Report_ProductBarcodesInfo_ProductId   FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
