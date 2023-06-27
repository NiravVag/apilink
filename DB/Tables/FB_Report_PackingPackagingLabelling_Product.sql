CREATE TABLE FB_Report_PackingPackagingLabelling_Product(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	PackingType [int] NULL,
	FbReportdetailsId [int] NULL,		
	SampleSizeCtns int NULL,
	Critical int NULL,
	Major int NULL,
	Minor int NULL,
	TotalDefectiveUnits int NULL,
	CreatedOn datetime,
	CartonQty INT,
	CONSTRAINT FK_FB_Report_PackingPackagingLabelling_Product_FbReportdetailsId FOREIGN KEY (FbReportdetailsId) REFERENCES FB_Report_Details(Id)
)