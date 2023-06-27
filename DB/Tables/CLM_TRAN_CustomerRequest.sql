CREATE TABLE [dbo].[CLM_TRAN_CustomerRequest]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Claimid] INT NULL, 
    [CustomerRequestId] INT NULL, 
    [Active] BIT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
    FOREIGN KEY (ClaimId) REFERENCES CLM_Transaction(Id),
    FOREIGN KEY ([CustomerRequestId]) REFERENCES [CLM_REF_CustomerRequest](Id),
    FOREIGN KEY ([CreatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([UpdatedBy]) REFERENCES It_UserMaster(Id),
    FOREIGN KEY ([DeletedBy]) REFERENCES It_UserMaster(Id)
)
