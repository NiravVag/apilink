CREATE TABLE [dbo].[EM_ExchangeRate](
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[CurrencyId1] [int] NOT NULL,
	[Currencyid2] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[User_id] [int] NOT NULL,
	[Rate] [float] NOT NULL,
	[Active] [bit] NOT NULL DEFAULT(1),
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
	[EntityId] INT NOT NULL,
	[ExRateTypeId] INT NULL,
	FOREIGN KEY([CurrencyId1]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([Currencyid2]) REFERENCES [dbo].[REF_Currency](Id),
	FOREIGN KEY([User_id]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY ([ExRateTypeId]) REFERENCES [dbo].[EM_ExchangeRateType](Id)
) 