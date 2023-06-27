
	CREATE TABLE [dbo].[FB_Report_Defect_Photos]
	(
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
		[DefectId] int not null,
		[Description] NVARCHAR(1500) NULL, 
		[Path] NVARCHAR(1000) NULL, 	
		[CreatedOn] DATETIME NULL,
		[DeletedOn] DATETIME NULL, 
		[Active] BIT NULL,	
		CONSTRAINT FK_FB_Report_Defect_Photos_FbDefectId
		FOREIGN KEY(DefectId) REFERENCES [FB_Report_InspDefects](Id)
	)