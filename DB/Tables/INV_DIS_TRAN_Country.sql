CREATE TABLE [dbo].[INV_DIS_TRAN_Country](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DiscountId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	CONSTRAINT INV_DIS_TRAN_Country_DiscountId FOREIGN KEY ([DiscountId]) REFERENCES [dbo].[INV_DIS_TRAN_Details](Id),
	CONSTRAINT INV_DIS_TRAN_Country_CountryId FOREIGN KEY(CountryId) REFERENCES [dbo].[REF_Country](Id),
	CONSTRAINT INV_DIS_TRAN_Country_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_DIS_TRAN_Country_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

