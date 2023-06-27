 CREATE TABLE INV_TRAN_Invoice_Request
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [CuPriceCardId] INT NULL,
	  [BilledName] NVARCHAR(100),
	  [BilledAddress] NVARCHAR(max),
	  [DepartmentId] INT NULL,
	  [BrandId] INT NULL,
	  [BuyerId] INT NULL,
	  [Active] BIT,
	  [CreatedBy] INT NULL, 
      [CreatedOn] DATETIME NULL, 
      [UpdatedBy] INT NULL, 
      [UpdatedOn] DATETIME NULL, 	
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,
	  ProductCategoryId INT NULL,

	  CONSTRAINT INV_TRAN_Invoice_Request_CuPriceCardId FOREIGN KEY ([CuPriceCardId]) REFERENCES [dbo].[CU_PR_Details](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[CU_ProductCategory](Id)
   )