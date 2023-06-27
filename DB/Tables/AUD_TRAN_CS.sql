CREATE TABLE [dbo].[AUD_TRAN_CS]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Audit_Id] INT NOT NULL, 
	[Staff_Id] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedTime] DATETIME NULL, 
    [CreatedBy] INT NULL, 
    [DeletedTime] DATETIME NULL, 
    [DeletedBy] INT NULL, 
	IsReport bit,
    FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY(Staff_Id) REFERENCES [HR_Staff](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [HR_Staff](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [HR_Staff](Id)
)
