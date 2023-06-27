CREATE TABLE [dbo].[SCH_Schedule_CS]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[BookingId] INT NOT NULL, 
    [CSId] INT NOT NULL,
	[ServiceDate] DATETIME NOT NULL,
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[IsReportReviewCS] bit 
	FOREIGN KEY (CSId) REFERENCES [dbo].[HR_STAFF](Id),
	FOREIGN KEY (BookingId) REFERENCES [dbo].[Insp_Transaction](Id),
	FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (ModifiedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)

)
