
CREATE TABLE [dbo].[INSP_PurchaseOrder_Color_Transaction]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[ColorCode] NVARCHAR(50),
	[ColorName] NVARCHAR(50),
	[PoTransId] INT,
	[ProductRefId] INT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	[Active] BIT,
	[EntityId] INT,
	PickingQuantity INT,
	BookingQuantity INT,
	CONSTRAINT FK_Color_Trans_Po_Trans_Id FOREIGN KEY([PoTransId]) REFERENCES [dbo].[INSP_PurchaseOrder_Transaction] (Id),
	CONSTRAINT FK_Color_Trans_Product_Trans_Id FOREIGN KEY([ProductRefId]) REFERENCES [dbo].[INSP_Product_Transaction] (Id),
	CONSTRAINT FK_Color_Trans_Created_By FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] (Id),
	CONSTRAINT FK_Color_Trans_Updated_By FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] (Id),
	CONSTRAINT FK_Color_Trans_Deleted_By FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] (Id),
	CONSTRAINT FK_Color_Trans_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)

