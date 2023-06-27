CREATE TABLE [dbo].[INV_TRAN_Bank_Tax]
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Tax_name] [nvarchar](500) NOT NULL,
	[Tax_Value] [decimal](18,2) NOT NULL, 
	[Active] [bit] NOT NULL,
	[BankId] [int] NOT NULL,
	[From_Date] [DATETIME] NOT NULL,
	[To_Date] [DATETIME] NULL,

	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 

    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
	
	[DeletedBy] [INT] NULL, 
    [DeletedOn] [DATETIME] NULL,  
	
	CONSTRAINT FK_INV_Bank_Tax_BankId	FOREIGN KEY ([BankId]) REFERENCES [dbo].[INV_REF_Bank](Id),
	CONSTRAINT FK_INV_Bank_Tax_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_Tax_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_Tax_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)

)