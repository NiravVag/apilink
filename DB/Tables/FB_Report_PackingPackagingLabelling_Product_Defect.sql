CREATE TABLE FB_Report_PackingPackagingLabelling_Product_Defect(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	PackingPackagingLabelling_Id [int] NULL,		
	[Code] [nvarchar](500) NULL,
	[RDNumber] [nvarchar](500) NULL,
	PackingType int NULL,
	[Description] [nvarchar](500) NULL,
	[Severity] [nvarchar](500) NULL,
	[Quantity] [int] NULL,
	[CreatedOn] [datetime] NULL,	
	CONSTRAINT FK_FB_Report_PackingPackagingLabelling_Product_Defect_PackingPackagingLabelling_Id FOREIGN KEY (PackingPackagingLabelling_Id) REFERENCES FB_Report_PackingPackagingLabelling_Product(Id)
)