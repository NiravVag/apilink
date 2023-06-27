CREATE TABLE FB_Report_FabricControlmadeWith(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ReportDetailsId INT NOT NULL,
	Name nvarchar(500) NULL,
	Active BIT,
	CreatedOn DATETIME,
	DeletedOn DATETIME,
	CONSTRAINT FB_Report_FabricControlmadeWith_ReportDetailsId FOREIGN KEY(ReportDetailsId) REFERENCES FB_Report_Details(Id)
)