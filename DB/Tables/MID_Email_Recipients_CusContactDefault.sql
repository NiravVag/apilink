CREATE TABLE [dbo].[MID_Email_Recipients_CusContactDefault]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[EmailTypeId] INT NULL,
	[Cus_ContactId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	FOREIGN KEY([EmailTypeId]) REFERENCES [MID_EmailTypes](Id), 
	FOREIGN KEY([Cus_ContactId]) REFERENCES [CU_Contact](Id), 
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
