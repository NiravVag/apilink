CREATE TABLE [dbo].[ENT_Feature_Details]
(
	[Id] int not null primary key identity(1,1),
	[FeatureId] [int],	
	[EntityId] [int],	
	[CountryId] [int],	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	[Active] [bit] NULL,
	CONSTRAINT [FK_ENT_Feature_Details_FeatureId] FOREIGN KEY([FeatureId]) REFERENCES [dbo].[ENT_REF_Features] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_CountryId] FOREIGN KEY([CountryId]) REFERENCES [dbo].[REF_Country] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)
