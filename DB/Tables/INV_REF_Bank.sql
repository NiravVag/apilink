CREATE TABLE [dbo].[INV_REF_Bank]
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](500) NULL,
	[AccountNumber] [nvarchar](500) NULL,
	[BankName] [nvarchar](500) NULL,
	[SwiftCode] [nvarchar](500) NULL,
	[BankAddress] [nvarchar](1000) NULL,
	[AccountCurrency] [int] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[Active] [bit] NULL,

	[ChopFileUniqueId] [nvarchar](255) NULL,
	[SignatureFileUniqueId] [nvarchar](255) NULL,

	[ChopFilename] [nvarchar](255) NULL,
	[SignatureFilename] [nvarchar](255) NULL,

	[ChopFileUrl] [nvarchar](max) NULL, 
	[SignatureFileUrl] [nvarchar](max) NULL, 

	[CreatedBy] [INT] NULL, 
    [CreatedOn] [DATETIME] NULL, 

    [UpdatedBy] [INT] NULL, 
    [UpdatedOn] [DATETIME] NULL,   

	[DeletedBy] [INT] NULL, 
    [DeletedOn] [DATETIME] NULL,  

	[EntityId] [INT] NULL, 
	[BillingEntity] INT NULL,

    CONSTRAINT FK_INV_Bank_BillingEntity   FOREIGN KEY ([BillingEntity]) REFERENCES [dbo].[REF_Billing_Entity](Id),
	CONSTRAINT FK_INV_Bank_AccountCurrency   FOREIGN KEY ([AccountCurrency]) REFERENCES [dbo].[REF_Currency](Id),
	CONSTRAINT FK_INV_Bank_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)