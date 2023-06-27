
CREATE TABLE [dbo].[REP_FAST_Template_Config](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TemplateId] [int] NULL,
	[ServiceTypeId] [int] NULL,
	[ProductCategoryId] [int] NULL,
	[IsStandardTemplate] [bit] NULL,
	[ScheduleFromDate] [datetime] NULL,
	[ScheduleToDate] [datetime] NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
	[BrandId] [int] NULL,
	[DepartId] [int] NULL,
	[FileExtensionID] [int] NULL,
	[ReportToolTypeID] [int] NULL,
	[Entityid] [int] NULL,
 CONSTRAINT [PK_REP_FAST_Template_Config] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_IsStandardTemplate]  DEFAULT ((1)) FOR [IsStandardTemplate]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_ReportToolType]  DEFAULT ((1)) FOR [ReportToolTypeID]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_AP_Entity] FOREIGN KEY([Entityid])
REFERENCES [dbo].[AP_Entity] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_AP_Entity]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[CU_Brand] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Brand]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Customer]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Department] FOREIGN KEY([DepartId])
REFERENCES [dbo].[CU_Department] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Department]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_File_Extension] FOREIGN KEY([FileExtensionID])
REFERENCES [dbo].[REF_File_Extension] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_File_Extension]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_ProductCategory] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[REF_ProductCategory] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_ProductCategory]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_ServiceType] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[REF_ServiceType] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_ServiceType]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REP_FAST_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[REP_FAST_Template] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REP_FAST_Template]
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REP_REF_ToolType] FOREIGN KEY([ReportToolTypeID])
REFERENCES [dbo].[REP_REF_ToolType] ([Id])
GO

ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REP_REF_ToolType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 pdf, 2 excel , 3 word ,4 ppt' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REP_FAST_Template_Config', @level2type=N'COLUMN',@level2name=N'FileExtensionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 FastReport , 2 Aspose' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REP_FAST_Template_Config', @level2type=N'COLUMN',@level2name=N'ReportToolTypeID'
GO


