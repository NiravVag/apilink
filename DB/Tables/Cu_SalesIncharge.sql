CREATE TABLE [dbo].[Cu_SalesIncharge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[StaffId] [int] NULL,
	[Active] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Cu_SalesIncharge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Cu_SalesIncharge]  WITH CHECK ADD  CONSTRAINT [FK_Cu_SalesIncharge_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO

ALTER TABLE [dbo].[Cu_SalesIncharge] CHECK CONSTRAINT [FK_Cu_SalesIncharge_CU_Customer]
GO

ALTER TABLE [dbo].[Cu_SalesIncharge]  WITH CHECK ADD  CONSTRAINT [FK_Cu_SalesIncharge_HR_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[HR_Staff] ([Id])
GO

ALTER TABLE [dbo].[Cu_SalesIncharge] CHECK CONSTRAINT [FK_Cu_SalesIncharge_HR_Staff]