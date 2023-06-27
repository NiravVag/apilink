CREATE TABLE [dbo].[CU_CustomerBusinessCountry]
(
	[customer_id] [int] NOT NULL,
	[business_country_id] [int] NOT NULL,
	[EntityId] INT NULL,
	PRIMARY KEY([customer_id], [business_country_id]),
	FOREIGN KEY([business_country_id]) REFERENCES [dbo].[REF_COUNTRY](Id),
	FOREIGN KEY ([customer_id]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT CU_CustomerBusinessCountry_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
