CREATE TABLE [dbo].[SCH_Schedule_CS]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BookingId] INT NOT NULL, 
    [CSId] INT NOT NULL,
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL
	FOREIGN KEY (BookingId) REFERENCES [dbo].[Insp_Transaction](Id),
	FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (ModifiedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)
