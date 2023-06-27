CREATE TABLE [dbo].[CU_CustomerSalesCountries]
(
	[customer_id] [int] NOT NULL,
	[sales_country_id] [int] NOT NULL,
	[EntityId] INT NULL,
	PRIMARY KEY([customer_id], [sales_country_id]),
	FOREIGN KEY([sales_country_id]) REFERENCES [dbo].[REF_COUNTRY](Id),
	FOREIGN KEY ([customer_id]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT CU_CustomerSalesCountries_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
