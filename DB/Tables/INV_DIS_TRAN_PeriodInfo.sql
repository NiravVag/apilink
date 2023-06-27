CREATE TABLE [dbo].[INV_DIS_TRAN_PeriodInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DiscountId] [int] NOT NULL,
	[LimitFrom] [decimal](18, 0) NOT NULL,
	[LimitTo] [decimal](18, 0) NOT NULL,
	[NotificationSent] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	CONSTRAINT INV_DIS_TRAN_PeriodInfo_DiscountId FOREIGN KEY ([DiscountId]) REFERENCES [dbo].[INV_DIS_TRAN_Details](Id),
	CONSTRAINT INV_DIS_TRAN_PeriodInfo_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_DIS_TRAN_PeriodInfo_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)