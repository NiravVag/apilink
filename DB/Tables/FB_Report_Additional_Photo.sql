CREATE TABLE [dbo].[FB_Report_Additional_Photos]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
    [FbReportDetailId] int not null,
	[ProductId] int NULL,
	[Description] NVARCHAR(1500) NULL, 
	[PhotoPath] NVARCHAR(1000) NULL, 	
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportDetailId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_InspSummary_Additional_Photos_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
