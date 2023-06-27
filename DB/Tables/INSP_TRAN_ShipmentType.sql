CREATE TABLE [dbo].[INSP_TRAN_ShipmentType](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[ShipmentTypeId] [int] NULL,
	[InspectionId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	CONSTRAINT [FK_INSP_SHIPMENT_TYPE] FOREIGN KEY([ShipmentTypeId]) REFERENCES [dbo].[INSP_REF_ShipmentType] ([Id]),
	CONSTRAINT [FK_INSP_TRANSACTION] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[INSP_Transaction] ([Id]),
	CONSTRAINT [FK_INSP_SHIPMENT_CREATED_BY] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_SHIPMENT_DELETED_BY] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)