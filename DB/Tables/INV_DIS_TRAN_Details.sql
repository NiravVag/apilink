CREATE TABLE [dbo].[INV_DIS_TRAN_Details]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerId] [int] NOT NULL,
	[DiscountType] [int] NOT NULL,
	[SelectAllCountry]  [bit] NULL,
	[PeriodFrom] [date] NULL,
	[PeriodTo] [date] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	CONSTRAINT INV_DIS_TRAN_Details_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT INV_DIS_TRAN_Details_DiscountType FOREIGN KEY ([DiscountType]) REFERENCES [dbo].[INV_DIS_REF_Type](Id),
	CONSTRAINT INV_DIS_TRAN_Details_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_DIS_TRAN_Details_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_DIS_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
