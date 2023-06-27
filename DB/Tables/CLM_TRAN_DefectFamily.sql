CREATE TABLE [dbo].[CLM_TRAN_DefectFamily]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ClaimId] INT NULL, 
    [DefectFamilyId] INT NULL, 
    [Active] INT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
    FOREIGN KEY (ClaimId) REFERENCES CLM_Transaction(Id),
    FOREIGN KEY ([DefectFamilyId]) REFERENCES [CLM_REF_DefectFamily](Id),
    FOREIGN KEY ([CreatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([UpdatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([DeletedBy]) REFERENCES It_UserMaster(Id)
)
