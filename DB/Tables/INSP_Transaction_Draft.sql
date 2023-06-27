﻿CREATE TABLE [dbo].[INSP_Transaction_Draft](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CustomerId] [int] NULL,
	[SupplierId] [int] NULL,
	[FactoryId] [int] NULL,
	[ServiceDateFrom] [datetime] NULL,
	[ServiceDateTo] [datetime] NULL,
	[BrandId] [int] NULL,
	[DepartmentId] [int] NULL,
	[InspectionId] [int] NULL,
	[BookingInfo] [nvarchar](max) NULL,
	[Active] [BIT] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	IsReinspectionDraft BIT NULL,
	CONSTRAINT [FK_INSP_Transaction_Draft_CustomerId] FOREIGN KEY([CustomerId])REFERENCES [dbo].[CU_Customer] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_SupplierId] FOREIGN KEY([SupplierId]) REFERENCES [dbo].[SU_Supplier] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_FactoryId] FOREIGN KEY([FactoryId]) REFERENCES [dbo].[SU_Supplier] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_BrandId] FOREIGN KEY([BrandId]) REFERENCES [dbo].[CU_Brand] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_DepartmentId] FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[CU_Department] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[INSP_Transaction] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_DeletedBy] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
	)