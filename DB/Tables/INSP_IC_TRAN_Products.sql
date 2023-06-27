CREATE TABLE [dbo].[INSP_IC_TRAN_Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ICId] INT,
	[ShipmentQty] INT,
	[BookingProductId] INT,
	[Active] BIT,
	[CreatedOn] DATETIME DEFAULT GETDATE(),
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	PoColorId int,
	CONSTRAINT FK_INSP_IC_TRAN_Products_BookingProductId FOREIGN KEY ([BookingProductId]) REFERENCES [dbo].[INSP_PurchaseOrder_Transaction](Id),
	CONSTRAINT FK_INSP_IC_TRAN_Products_ICId FOREIGN KEY ([ICId]) REFERENCES [dbo].[INSP_IC_Transaction](Id),
	CONSTRAINT FK_INSP_IC_TRAN_Products_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INSP_IC_TRAN_Products_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INSP_IC_TRAN_Products_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	Constraint FK_INSP_IC_TRAN_Products_PoColorId FOREIGN KEY (PoColorId) REFERENCES INSP_PurchaseOrder_Color_Transaction(Id)
)
