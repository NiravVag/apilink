CREATE TABLE [dbo].[DA_UserByFactoryCountry](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DaUserCustomerId] [int] NOT NULL,
	[FactoryCountryId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[EntityId] [int] NOT NULL,
	CONSTRAINT [FK_DA_UserByFactoryCountry_AP_Entity_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_DA_UserCustomer_DaUserCustomerId] FOREIGN KEY([DaUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_IT_UserMaster_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_REF_Country_FactoryCountryId] FOREIGN KEY([FactoryCountryId]) REFERENCES [dbo].[REF_Country] ([Id])
)