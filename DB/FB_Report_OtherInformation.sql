CREATE TABLE [dbo].[FB_Report_OtherInformation]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SubCategory] NVARCHAR(1000) NULL,		
	[Sub_Category2] nvarchar(1000) NULL,
	[Remarks] NVARCHAR(max) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_OtherInformation_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
