CREATE TABLE [dbo].[EC_ExpenseClaimsInspection]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [BookingId] INT NOT NULL, 
	[EntityId] INT NULL, 
    [ExpenseClaimDetailId] INT NOT NULL, 
    [Active] BIT NOT NULL,
	[CreatedOn] DATETIME NULL, 
    [CreatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	FOREIGN KEY ([BookingId]) REFERENCES [INSP_Transaction](Id),
	FOREIGN KEY ([ExpenseClaimDetailId]) REFERENCES [EC_ExpensesClaimDetais](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT EC_ExpenseClaimsInspection_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)
