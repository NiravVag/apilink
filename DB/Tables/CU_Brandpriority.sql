CREATE TABLE [dbo].[CU_Brandpriority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[BrandpriorityId] [int] NULL,
	[Active] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_CU_Brandpriority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CU_Brandpriority]  WITH CHECK ADD  CONSTRAINT [FK_CU_Brandpriority_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO

ALTER TABLE [dbo].[CU_Brandpriority] CHECK CONSTRAINT [FK_CU_Brandpriority_CU_Customer]
GO

ALTER TABLE [dbo].[CU_Brandpriority]  WITH CHECK ADD  CONSTRAINT [FK_CU_Brandpriority_Cu_REF_BrandPriority] FOREIGN KEY([BrandpriorityId])
REFERENCES [dbo].[Cu_REF_BrandPriority] ([Id])
GO

ALTER TABLE [dbo].[CU_Brandpriority] CHECK CONSTRAINT [FK_CU_Brandpriority_Cu_REF_BrandPriority]