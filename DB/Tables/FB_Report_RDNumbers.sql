CREATE TABLE FB_Report_RDNumbers(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ReportdetailsId] [int] NULL,
	[ProductId] [int] NULL,
	[RDNumber] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NULL,
	[PoId] [int] NULL,
	[PoColorId] [int] NULL,
	CONSTRAINT FK_FB_Report_RDNumbers_ReportdetailsId FOREIGN KEY (ReportDetailsId) REFERENCES FB_Report_Details(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_ProductId FOREIGN KEY (ProductId) REFERENCES INSP_Product_Transaction(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_PoColorId FOREIGN KEY (PoColorId) REFERENCES INSP_PurchaseOrder_Color_Transaction(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_PoId FOREIGN KEY (PoId) REFERENCES INSP_PurchaseOrder_Transaction(Id)
)
