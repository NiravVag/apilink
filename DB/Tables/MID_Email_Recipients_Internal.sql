CREATE TABLE [dbo].[MID_Email_Recipients_Internal]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[EmailConfigId] INT NOT NULL,
	[Internal_ContactId] INT NOT NULL,
	[IsAccounting] BIT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	FOREIGN KEY([EmailConfigId]) REFERENCES [MID_Email_Recipients_Configuration](Id), 
	FOREIGN KEY([Internal_ContactId]) REFERENCES [HR_Staff](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
