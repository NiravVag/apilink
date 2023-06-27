CREATE TABLE [dbo].[FB_Report_Comments]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductId] INT NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	[Category] nvarchar(1000) NULL,
	[Sub_Category] nvarchar(1000) NULL,
	[Sub_Category2] nvarchar(1000) NULL,
	[CustomerReferenceCode] NVARCHAR(1000) NULL, 
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Report_Comments_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
