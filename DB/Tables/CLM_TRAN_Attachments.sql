CREATE TABLE [dbo].[CLM_TRAN_Attachments]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UniqueId] NVARCHAR(2000) NULL, 
    [ClaimId] INT NULL, 
    [FileType] INT NULL, 
    [FileName] NVARCHAR(1000) NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
    [FileUrl] NVARCHAR(MAX) NULL, 
    [EntityId] INT NULL, 
    [Active] BIT NULL, 
    [FileDesc] NVARCHAR(MAX) NULL,
    FOREIGN KEY ([CreatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([UpdatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([DeletedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([ClaimId]) REFERENCES CLM_Transaction(Id),
    FOREIGN KEY ([FileType]) REFERENCES CLM_REF_FileType(Id),
	FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

)
