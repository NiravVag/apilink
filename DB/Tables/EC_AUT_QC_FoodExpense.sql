
CREATE TABLE [dbo].[EC_AUT_QC_FoodExpense]
(
    [Id] int not null primary key identity(1,1),
	[QcId] [int],	
	[InspectionId] [int],		
	[FactoryCountry] [int],		
	[ServiceDate] [DateTime],	

	[FoodAllowance] [float],	
	[FoodAllowanceCurrency] [int],	

	
	[Active] [bit] NULL,
	[EntityId] [int],

	[IsExpenseCreated] [bit] NULL,
	[IsFoodAllowanceConfigured] [bit] NULL,

	[Comments] nvarchar(2500),

	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	
	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),	

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_FactoryCountry] FOREIGN KEY([FactoryCountry]) REFERENCES [dbo].[Ref_Country] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_FoodAllowanceCurrency] FOREIGN KEY([FoodAllowanceCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])
)

