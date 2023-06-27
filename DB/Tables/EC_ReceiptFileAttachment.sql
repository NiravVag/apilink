CREATE TABLE [dbo].[EC_ReceiptFileAttachment]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ExpenseId] INT NOT NULL, 
	[UniqueId] NVARCHAR(1000) NULL, 
    [FileName] NVARCHAR(500) NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [Createdby] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [Active] BIT NOT NULL,
    FOREIGN KEY ([ExpenseId]) REFERENCES [dbo].EC_ExpensesClaimDetais (Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
