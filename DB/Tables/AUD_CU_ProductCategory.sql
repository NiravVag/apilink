CREATE TABLE [dbo].[AUD_CU_ProductCategory](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[ServiceType] [int] NULL,
	[CustomerId] [int] NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NULL,
	[FB_Name] [nvarchar](200) NULL,
	CONSTRAINT [FK_AUD_CU_ProductCategory_ServiceType] FOREIGN KEY([ServiceType]) REFERENCES [dbo].[REF_ServiceType] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_CustomerId] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_Customer] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_DeletedBy] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id])
)