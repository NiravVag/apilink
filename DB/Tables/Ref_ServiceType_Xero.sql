CREATE TABLE [dbo].[Ref_ServiceType_Xero]
    (
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
		[TrackingOptionName] [nvarchar](500) NULL,
		[XERO_Account] [nvarchar](500) NULL,
		[Inspection_Type_consolidate] [nvarchar](500) NULL,
		[Inspection_ServiceType_Id] [int] NULL,
		[Inspection_Type] [nvarchar](500) NULL,
		[Entity_Id] [INT] NULL,
		[Active] [bit] NULL,
		[TrackingOptionName_Travel] [nvarchar](500) NULL,
		CONSTRAINT FK_Ref_ServiceType_Xero_Entity_Id FOREIGN KEY(Entity_Id) REFERENCES [dbo].[AP_Entity],
		CONSTRAINT FK_Ref_ServiceType_Xero_Inspection_ServiceType_Id 
		FOREIGN KEY(Inspection_ServiceType_Id) REFERENCES [dbo].[REF_ServiceType]
	)
