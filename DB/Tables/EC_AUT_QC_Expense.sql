CREATE TABLE [dbo].[EC_AUT_QC_Expense]
(
	[Id] int not null primary key identity(1,1),
	[InspectionId] [int],	
	[QcId] [int],	
	[StartPort] [int],	
	[FactoryTown] [int],	
	[TripType] [int],	
	[ServiceDate] [DateTime],	
	[TravelTariff] [float],	
	[TravelTariffCurrency] [int],	
	[FoodAllowance] [float],	
	[FoodAllowanceCurrency] [int],	
	[EntityId] [int],
	[Active] [bit] NULL,
	[IsExpenseCreated] [bit] NULL,
	[IsTravelAllowanceConfigured] [bit] NULL,
	[IsFoodAllowanceConfigured] [bit] NULL,
	[Comments] nvarchar(2500),
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	
	CONSTRAINT [FK_EC_AUT_QC_Expense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_StartPort] FOREIGN KEY([StartPort]) REFERENCES [dbo].[EC_AUT_REF_StartPort] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_FactoryTown] FOREIGN KEY([FactoryTown]) REFERENCES [dbo].[Ref_Town] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_TripType] FOREIGN KEY([TripType]) REFERENCES [dbo].[EC_AUT_REF_TripType] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_TravelTariffCurrency] FOREIGN KEY([TravelTariffCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_Expense_FoodAllowanceCurrency] FOREIGN KEY([FoodAllowanceCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])
)
