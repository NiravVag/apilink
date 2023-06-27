CREATE TABLE CU_Report_CustomerDecisionComment (
	Id [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerId [int] NOT NULL,
	ReportResult [nvarchar](1000) NULL,
	Comments [nvarchar](max) NULL,
	Active [bit] NOT NULL,
	CONSTRAINT [FK_CU_Customer_CU_Report_CustomerDecisionComment_CustomerId] FOREIGN KEY(CustomerId) REFERENCES CU_Customer (Id)
)