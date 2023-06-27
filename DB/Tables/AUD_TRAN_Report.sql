CREATE TABLE [dbo].[AUD_TRAN_Report]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UniqueId] NVARCHAR(1000) NULL, 
    [AuditId] INT NOT NULL, 
    [FileName] NVARCHAR(500) NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY ([AuditId]) REFERENCES [dbo].[AUD_Transaction](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
