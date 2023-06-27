CREATE TABLE [dbo].[FB_Report_SampleTypes]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SampleType] NVARCHAR(1000) NULL,
	[Description] NVARCHAR(max) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_SampleTypeProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)
