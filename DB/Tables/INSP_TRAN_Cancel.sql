CREATE TABLE [dbo].[INSP_TRAN_Cancel]
(
	[Id] INT NOT NULL  IDENTITY(1,1) PRIMARY KEY,
	[ReasonTypeId] [int] NOT NULL,
	[TimeTypeId] [int] NULL,
	[TravellingExpense] [decimal](18, 2) NULL,
	[CurrencyId] [int] NULL,
	[Comments] [nvarchar](500) NULL,
	[InternalComments] [nvarchar](500) NULL,
	[Inspection_Id] [int] NOT NULL,
	[CreatedOn] DATETIME NULL default GETDATE(), 	
    [CreatedBy] INT NULL, 
	[ModifiedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
	FOREIGN KEY(Inspection_Id) REFERENCES [INSP_Transaction](Id),
FOREIGN KEY(CurrencyId) REFERENCES [REF_Currency](Id),
FOREIGN KEY(ReasonTypeId) REFERENCES [INSP_Cancel_Reasons](Id),
FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id)
)
