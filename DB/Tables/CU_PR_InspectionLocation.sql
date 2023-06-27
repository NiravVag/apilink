CREATE TABLE [dbo].[CU_PR_InspectionLocation]
(
	[Id] int not null primary key identity(1,1),
	[Cu_Price_Id] [int],	
	[InspectionLocationId] [int],		
	[Active] [bit] NULL,	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,	
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	CONSTRAINT [FK_CU_PR_InspectionLocation_Cu_Price_Id] FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details] ([Id]),
	CONSTRAINT [FK_CU_PR_InspectionLocation_InspectionLocationId] FOREIGN KEY([InspectionLocationId]) REFERENCES [dbo].[INSP_REF_InspectionLocation] ([Id]),	
	CONSTRAINT [FK_CU_PR_InspectionLocation_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_CU_PR_InspectionLocation_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)
