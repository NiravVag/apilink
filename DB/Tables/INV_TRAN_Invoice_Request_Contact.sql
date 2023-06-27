CREATE TABLE INV_TRAN_Invoice_Request_Contact
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [CuPriceCardId] INT NULL,
	  [InvoiceRequestId] INT NULL,	 
	  [ContactId] INT NULL,
	  [IsCommon] BIT,
	  [Active] BIT,
	  [CreatedBy] INT NULL, 
      [CreatedOn] DATETIME NULL, 
      [UpdatedBy] INT NULL, 
      [UpdatedOn] DATETIME NULL, 	
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_CuPriceCardId FOREIGN KEY ([CuPriceCardId]) REFERENCES [dbo].[CU_PR_Details](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_InvoiceRequestId FOREIGN KEY ([InvoiceRequestId]) REFERENCES [dbo].[INV_TRAN_Invoice_Request](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_ContactId FOREIGN KEY ([ContactId]) REFERENCES [dbo].[CU_Contact](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
   )