CREATE TABLE [dbo].[INSP_PurchaseOrder_Transaction]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[PO_Id] INT NOT NULL,

	[Container_Ref_Id] INT  NULL, 
	[Product_Ref_Id] INT NOT NULL,
    [Inspection_Id] INT NOT NULL, 
	[BookingQuantity] INT NOT NULL, 
	[PickingQuantity] INT  NULL, 	
	[Remarks] NVARCHAR(max) NULL, 	

	[Destination_Country_Id] INT NULL, 
	[ETD] DATETIME NULL,
	[CustomerReferencePo] NVARCHAR(1000) NULL,
	[Fb_Mission_Product_Id] INT NULL, 
	
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
	[UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 

    [Active] BIT NULL,	

	[EntityId] INT NULL,


	FOREIGN KEY([Product_Ref_Id]) REFERENCES [dbo].[INSP_Product_Transaction](Id),
	FOREIGN KEY([Container_Ref_Id]) REFERENCES [dbo].[INSP_Container_Transaction](Id),
    FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([PO_Id]) REFERENCES [dbo].[CU_PurchaseOrder](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    FOREIGN KEY (Destination_Country_Id) REFERENCES [dbo].[REF_Country] (Id),
	CONSTRAINT FK_INSP_PurchaseOrder_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
