﻿
CREATE TABLE [dbo].[EC_ExpensesClaimDetais](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[ExpenseId] [int] NOT NULL,
	[EntityId] INT NULL, 
	[Receipt] [bit] NOT NULL, -- TO CHECK
	[ExpenseDate] [datetime] NOT NULL,
	[ExpenseTypeId] [int] NOT NULL,
	[StartCityId] [int] NULL,
	[ArrivalCityId] [int] NULL,
	[CurrencyId] [int] NOT NULL,
	[Amount] [float] NOT NULL,
	[Ammount_HK] [float] NULL, -- To check
	[Description] [nvarchar](4000) NULL,
	[Exchange_Rate] [float] NULL,	
	[TransitTime] [float] NULL,
	[active] [bit] NOT NULL DEFAULT(1),
	[PayrollCurrencyId] [int] NULL,
	[AmountPayroll] [float] NULL,
	[PayrollExchangeRate] [float] NULL,
	[TripType] INT NULL,
	[Qc_Auto_ExpenseId] INT NULL,
	[IsAutoExpense] BIT NULL,
	[IsManagerApproved ] BIT NULL,
	[Qc_Travel_ExpenseId] INT NULL,
	[Qc_Food_ExpenseId] INT NULL,
	FOREIGN KEY([ExpenseId]) REFERENCES [dbo].[EC_ExpencesClaims](Id),
	FOREIGN KEY([ExpenseTypeId]) REFERENCES [dbo].[EC_ExpensesTypes](Id),
	FOREIGN KEY([StartCityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([ArrivalCityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([PayrollCurrencyId]) REFERENCES [dbo].[REF_Currency](Id),
	CONSTRAINT EC_ExpensesClaimDetais_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id),
	CONSTRAINT EC_ExpensesClaimDetais_TripType  FOREIGN KEY(TripType) REFERENCES [dbo].[EC_AUT_REF_TripType](Id),
	CONSTRAINT EC_ExpensesClaimDetais_Qc_Auto_ExpenseId  FOREIGN KEY(Qc_Auto_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_Expense](Id),
	CONSTRAINT EC_ExpensesClaimDetais_Qc_Travel_ExpenseId  FOREIGN KEY(Qc_Travel_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_TravelExpense](Id),
	CONSTRAINT EC_ExpensesClaimDetais_Qc_Food_ExpenseId  FOREIGN KEY(Qc_Food_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_FoodExpense](Id)
	)
