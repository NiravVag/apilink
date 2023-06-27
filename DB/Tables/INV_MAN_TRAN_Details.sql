CREATE TABLE[INV_MAN_TRAN_Details]
(
	 [Id] INT IDENTITY(1,1) PRIMARY KEY, 
	 [Inv_ManualId] INT NOT NULL,
	 [Description] NVARCHAR(1000),
	 [ServiceFee] FLOAT, 
	 [ExpChargeBack] FLOAT,
	 [OtherCost] FLOAT,
	 [Subtotal] FLOAT,
	 [Remarks] NVARCHAR(2000),
	 [Active] BIT,
	 [CreatedBy] INT,
	 [CreatedOn] DATETIME,
	 [DeletedBy] INT,
	 [DeletedOn] DATETIME,
	 [UpdatedBy] INT,
	 [UpdatedOn] DATETIME,
	 [Discount] FLOAT,
	 [UnitPrice] FLOAT,
	 [Manday] FLOAT,
	CONSTRAINT INV_MAN_TRAN_Details_Inv_ManualId FOREIGN KEY ([Inv_ManualId]) REFERENCES [dbo].[INV_MAN_Transaction](Id),
	CONSTRAINT INV_MAN_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[It_UserMaster](Id),
	CONSTRAINT INV_MAN_TRAN_Details_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[It_UserMaster](Id),
	CONSTRAINT INV_MAN_TRAN_Details_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[It_UserMaster](Id)
)

