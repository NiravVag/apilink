
CREATE TABLE [dbo].[EC_AUT_QC_TravelExpense]
(
    [Id] int not null primary key identity(1,1),
	[QcId] [int],		
	[InspectionId] [int],	
	[ServiceDate] [DateTime],	
	
	[StartPort] [int],	
	[FactoryTown] [int],	
	[TripType] [int],	
	[TravelTariff] [float],	
	[TravelTariffCurrency] [int],

	[Active] [bit] NULL,
	[EntityId] [int],
	
	[IsExpenseCreated] [bit] NULL,
	[IsTravelAllowanceConfigured] [bit] NULL,

	[Comments] nvarchar(2500),

	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	
	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_StartPort] FOREIGN KEY([StartPort]) REFERENCES [dbo].[EC_AUT_REF_StartPort] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_FactoryTown] FOREIGN KEY([FactoryTown]) REFERENCES [dbo].[Ref_Town] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_TripType] FOREIGN KEY([TripType]) REFERENCES [dbo].[EC_AUT_REF_TripType] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_TravelTariffCurrency] FOREIGN KEY([TravelTariffCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])	
)