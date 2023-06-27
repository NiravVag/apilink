CREATE TABLE [dbo].[MID_Email_Recipients_Configuration]
(
	[Id] INT NOT NULL  IDENTITY(1,1) PRIMARY KEY, 
	[EmailTypeId] INT NULL,
	[Is_CUS_BookingContact] BIT NULL,
	[Is_SUP_BookingContact] BIT NULL,
	[Is_FACT_BookingContact] BIT NULL,	
	[Active] BIT NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	FOREIGN KEY([EmailTypeId]) REFERENCES [MID_EmailTypes](Id), 
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
