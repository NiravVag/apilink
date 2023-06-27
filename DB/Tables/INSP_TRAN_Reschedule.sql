CREATE TABLE [dbo].[INSP_TRAN_Reschedule]
(
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ReasonTypeId] [int] NOT NULL,
	[TimeTypeId] [int] NULL,
	[TravellingExpense] [decimal](18, 2) NULL,
	[CurrencyId] [int] NULL,
	[Comments] [nvarchar](500) NULL,
	[InternalComments] [nvarchar](500) NULL,
	[Inspection_Id] [int] NOT NULL,
	[ServiceFromDate] DATETIME NOT NULL, 
	[ServiceToDate] DATETIME NOT NULL, 
	[CreatedOn] DATETIME NULL default GETDATE(), 	
    [CreatedBy] INT NULL, 
    [ModifiedOn] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
	CONSTRAINT FK_InspectionId FOREIGN KEY(Inspection_Id) REFERENCES [INSP_Transaction](Id),
CONSTRAINT FK_CurrencyId FOREIGN KEY(CurrencyId) REFERENCES [REF_Currency](Id),
CONSTRAINT FK_ReasonTypeId FOREIGN KEY(ReasonTypeId) REFERENCES [INSP_Reschedule_Reasons](Id),
CONSTRAINT FK_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_ModifiedBy FOREIGN KEY(ModifiedBy) REFERENCES [IT_UserMaster](Id)
)

